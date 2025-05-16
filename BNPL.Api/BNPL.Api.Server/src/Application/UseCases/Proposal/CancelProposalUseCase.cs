using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class CancelProposalUseCase(
        IProposalRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await repository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status is ProposalStatus.Signed or ProposalStatus.Formalized or ProposalStatus.Disbursed or ProposalStatus.Finalized)
                throw new InvalidOperationException("Proposal cannot be cancelled in its current state.");

            proposal.MarkAsCancelled(DateTime.UtcNow, userContext.UserId);

            await repository.UpdateAsync(proposal);

            return new ServiceResult<string>("Proposal cancelled successfully.");
        }
    }
}
