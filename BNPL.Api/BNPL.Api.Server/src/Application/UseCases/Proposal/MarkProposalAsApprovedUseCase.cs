using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsApprovedUseCase(
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<bool, string>.Fail("Proposal not found.");

            if (proposal.Status != Domain.Enums.ProposalStatus.Created)
                return Result<bool, string>.Fail("Proposal must be in 'Created' status to be marked as 'UnderAnalysis'.");

            proposal.MarkAsApproved(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal);

            return Result<bool, string>.Ok(true);
        }
    }
}
