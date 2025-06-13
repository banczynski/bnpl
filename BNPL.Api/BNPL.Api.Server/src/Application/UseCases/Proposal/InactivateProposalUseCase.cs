using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record InactivateProposalRequestUseCase(Guid ProposalId);

    public sealed class InactivateProposalUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository itemRepository,
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<InactivateProposalRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivateProposalRequestUseCase request)
        {
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status is ProposalStatus.Active)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.InvalidStateForInactivation);

            await proposalRepository.InactivateAsync(request.ProposalId, userId, unitOfWork.Transaction);

            var items = await itemRepository.GetByProposalIdAsync(request.ProposalId, unitOfWork.Transaction);
            foreach (var item in items)
            {
                item.IsActive = false;
                item.UpdatedAt = now;
                item.UpdatedBy = userId;
            }
            await itemRepository.UpdateManyAsync(items, unitOfWork.Transaction);

            var installments = await installmentRepository.GetByProposalIdAsync(request.ProposalId, unitOfWork.Transaction);
            foreach (var i in installments)
            {
                if (i.Status == Domain.Enums.InstallmentStatus.Paid)
                    continue;

                i.IsActive = false;
                i.UpdatedAt = now;
                i.UpdatedBy = userId;
            }
            await installmentRepository.UpdateManyAsync(installments, unitOfWork.Transaction);

            var invoices = await invoiceRepository.GetByProposalIdAsync(request.ProposalId, unitOfWork.Transaction);
            foreach (var invoice in invoices)
            {
                invoice.IsActive = false;
                invoice.UpdatedAt = now;
                invoice.UpdatedBy = userId;
            }
            await invoiceRepository.UpdateManyAsync(invoices, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}