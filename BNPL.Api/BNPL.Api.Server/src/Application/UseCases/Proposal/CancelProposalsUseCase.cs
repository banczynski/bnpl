using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using BNPL.Api.Server.src.Presentation.Configurations;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Microsoft.Extensions.Options;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record CancelProposalsRequestUseCase;

    public sealed class CancelProposalsUseCase(
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IOptions<BusinessRulesSettings> businessRules
    ) : IUseCase<CancelProposalsRequestUseCase, Result<int, Error>>
    {
        public async Task<Result<int, Error>> ExecuteAsync(CancelProposalsRequestUseCase request)
        {
            var cutoff = DateTime.UtcNow.AddDays(-businessRules.Value.MaxProposalCancellationDays);
            var proposals = await proposalRepository.GetPendingBeforeDateAsync(cutoff, unitOfWork.Transaction);

            int count = 0;
            foreach (var proposal in proposals)
            {
                if (proposal.Status is ProposalStatus.Active or ProposalStatus.Finalized or ProposalStatus.Cancelled)
                    continue;

                proposal.MarkAsCancelled(DateTime.UtcNow, userContext.GetRequiredUserId());
                count++;
            }

            await proposalRepository.UpdateManyAsync(proposals, unitOfWork.Transaction);

            return Result<int, Error>.Ok(count);
        }
    }
}