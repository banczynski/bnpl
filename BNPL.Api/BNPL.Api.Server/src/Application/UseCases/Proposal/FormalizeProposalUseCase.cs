using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class FormalizeProposalUseCase(
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status != ProposalStatus.Signed)
                throw new InvalidOperationException("Proposal must be signed before formalization.");

            proposal.MarkAsFormalized(DateTime.UtcNow, userContext.UserId);

            await proposalRepository.UpdateAsync(proposal);

            return new ServiceResult<string>("Proposal formalized successfully.");
        }
    }
}
