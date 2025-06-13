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
    public sealed record CancelProposalRequestUseCase(Guid ProposalId);

    public sealed class CancelProposalUseCase(
        IProposalRepository proposalRepository,
        IOptions<BusinessRulesSettings> businessRules,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CancelProposalRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(CancelProposalRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status is ProposalStatus.Active or ProposalStatus.Finalized or ProposalStatus.Cancelled)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.InvalidStateForCancellation);

            if (proposal.CreatedAt < DateTime.UtcNow.AddDays(-businessRules.Value.MaxProposalCancellationDays))
                return Result<bool, Error>.Fail(DomainErrors.Proposal.CancellationPeriodExpired);

            proposal.MarkAsCancelled(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}