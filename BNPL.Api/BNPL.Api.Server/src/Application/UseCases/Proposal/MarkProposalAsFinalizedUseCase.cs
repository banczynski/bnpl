using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record MarkProposalAsFinalizedRequestUseCase(Guid ProposalId);

    public sealed class MarkProposalAsFinalizedUseCase(
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<MarkProposalAsFinalizedRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(MarkProposalAsFinalizedRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != ProposalStatus.Active)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.MustBeActive);

            var installments = await installmentRepository.GetByProposalIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (installments.Any(i => i.Status != InstallmentStatus.Paid))
                return Result<bool, Error>.Fail(DomainErrors.Proposal.HasUnpaidInstallments);

            proposal.MarkAsFinalized(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}