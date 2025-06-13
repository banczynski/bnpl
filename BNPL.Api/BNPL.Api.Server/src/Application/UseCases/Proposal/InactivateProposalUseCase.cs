using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class InactivateProposalUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository itemRepository,
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();
                var now = DateTime.UtcNow;

                var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                if (proposal is null)
                    return Result<bool, string>.Fail("Proposal not found.");

                if (proposal.Status is ProposalStatus.Active)
                    return Result<bool, string>.Fail("Proposal cannot be inactive in its current state.");

                await proposalRepository.InactivateAsync(proposalId, userContext.GetRequiredUserId(), now, scope.Transaction);

                var items = await itemRepository.GetByProposalIdAsync(proposalId, scope.Transaction);
                foreach (var item in items)
                {
                    item.IsActive = false;
                    item.UpdatedAt = now;
                    item.UpdatedBy = userContext.GetRequiredUserId();
                }
                await itemRepository.UpdateManyAsync(items, scope.Transaction);

                var installments = await installmentRepository.GetByProposalIdAsync(proposalId, scope.Transaction);
                foreach (var i in installments)
                {
                    if (i.Status == Domain.Enums.InstallmentStatus.Paid)
                        continue;

                    i.IsActive = false;
                    i.UpdatedAt = now;
                    i.UpdatedBy = userContext.GetRequiredUserId();
                }
                await installmentRepository.UpdateManyAsync(installments, scope.Transaction);

                var invoices = await invoiceRepository.GetByProposalIdAsync(proposalId, scope.Transaction);
                foreach (var invoice in invoices)
                {
                    invoice.IsActive = false;
                    invoice.UpdatedAt = now;
                    invoice.UpdatedBy = userContext.GetRequiredUserId();
                }
                await invoiceRepository.UpdateManyAsync(invoices, scope.Transaction);

                scope.Commit();

                return Result<bool, string>.Ok(true);
            }
            catch 
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
