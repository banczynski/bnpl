using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsDisbursedUseCase(
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status != Domain.Enums.ProposalStatus.Formalized)
                throw new InvalidOperationException("Proposal must be formalized before disbursement.");

            proposal.MarkAsDisbursed(DateTime.UtcNow, userContext.UserId);

            await proposalRepository.UpdateAsync(proposal);

            return new ServiceResult<string>("Proposal marked as disbursed.");
        }
    }
}
