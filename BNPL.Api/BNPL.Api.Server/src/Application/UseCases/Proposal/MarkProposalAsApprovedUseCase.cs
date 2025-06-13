using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record MarkProposalAsApprovedRequestUseCase(Guid ProposalId);

    public sealed class MarkProposalAsApprovedUseCase(
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<MarkProposalAsApprovedRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(MarkProposalAsApprovedRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != Domain.Enums.ProposalStatus.Created)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.InvalidStatusForApproval);

            proposal.MarkAsApproved(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}