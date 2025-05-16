using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsFinalizedUseCase(
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status != ProposalStatus.Disbursed)
                throw new InvalidOperationException("Proposal must be disbursed before finalization.");

            var installments = await installmentRepository.GetByProposalIdAsync(proposalId);

            if (installments.Any(i => i.Status != InstallmentStatus.Paid))
                throw new InvalidOperationException("Proposal still has unpaid installments.");

            proposal.MarkAsFinalized(DateTime.UtcNow, userContext.UserId);

            await proposalRepository.UpdateAsync(proposal);

            return new ServiceResult<string>("Proposal marked as finalized.");
        }
    }
}
