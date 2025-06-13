using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsFinalizedUseCase(
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<bool, string>.Fail("Proposal not found.");

            if (proposal.Status != ProposalStatus.Active)
                return Result<bool, string>.Fail("Proposal must be active before finalization.");

            var installments = await installmentRepository.GetByProposalIdAsync(proposalId);
            if (installments.Any(i => i.Status != InstallmentStatus.Paid))
                return Result<bool, string>.Fail("Proposal still has unpaid installments.");

            proposal.MarkAsFinalized(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal);

            return Result<bool, string>.Ok(true);
        }
    }
}
