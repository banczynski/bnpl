using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class CancelProposalsUseCase(
        IProposalRepository proposalRepository,
        IUserContext userContext)
    {
        private const int MaxCancellationDays = 7;

        public async Task<Result<int, string>> ExecuteAsync()
        {
            var cutoff = DateTime.UtcNow.AddDays(-MaxCancellationDays);
            var proposals = await proposalRepository.GetPendingBeforeDateAsync(cutoff);

            int count = 0;
            foreach (var proposal in proposals)
            {
                if (proposal.Status is ProposalStatus.Active or ProposalStatus.Finalized or ProposalStatus.Cancelled)
                    continue;

                proposal.MarkAsCancelled(DateTime.UtcNow, userContext.GetRequiredUserId());
                count++;
            }

            await proposalRepository.UpdateManyAsync(proposals);

            return Result<int, string>.Ok(count);
        }
    }
}
