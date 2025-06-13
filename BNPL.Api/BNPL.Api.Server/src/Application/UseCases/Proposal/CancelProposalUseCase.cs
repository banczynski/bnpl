using BNPL.Api.Server.src.Application.Abstractions.Notification;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class CancelProposalUseCase(
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        private const int MaxCancellationDays = 7;

        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                if (proposal is null)
                    return Result<bool, string>.Fail("Proposal not found.");

                if (proposal.Status is ProposalStatus.Active or ProposalStatus.Finalized or ProposalStatus.Cancelled)
                    return Result<bool, string>.Fail("Proposal cannot be cancelled in its current state.");

                if (proposal.CreatedAt < DateTime.UtcNow.AddDays(-MaxCancellationDays))
                    return Result<bool, string>.Fail("The cancellation period has expired.");

                proposal.MarkAsCancelled(DateTime.UtcNow, userContext.GetRequiredUserId());
                await proposalRepository.UpdateAsync(proposal, scope.Transaction);

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