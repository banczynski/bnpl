using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using BNPL.Api.Server.src.Presentation.Configurations;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Microsoft.Extensions.Options;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed record MarkProposalItemAsReturnedRequestUseCase(Guid ProposalItemId, string Reason);

    public sealed class MarkProposalItemAsReturnedUseCase(
        IProposalItemRepository proposalItemRepository,
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IOptions<BusinessRulesSettings> businessRules
    ) : IUseCase<MarkProposalItemAsReturnedRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(MarkProposalItemAsReturnedRequestUseCase request)
        {
            var item = await proposalItemRepository.GetByIdAsync(request.ProposalItemId, unitOfWork.Transaction);
            if (item is null || item.Returned)
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.InvalidStateForReturn);

            var proposal = await proposalRepository.GetByIdAsync(item.ProposalId, unitOfWork.Transaction);
            if (proposal is null || proposal.Status is not (ProposalStatus.Approved or ProposalStatus.Active))
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotEligibleForItemReturn);

            if (proposal.CreatedAt < DateTime.UtcNow.AddDays(-businessRules.Value.MaxItemReturnDays))
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.ReturnPeriodExpired);

            item.Returned = true;
            item.ReturnedAt = DateTime.UtcNow;
            item.ReturnReason = request.Reason;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = userContext.GetRequiredUserId();

            await proposalItemRepository.UpdateAsync(item, unitOfWork.Transaction);
            return Result<bool, Error>.Ok(true);
        }
    }
}