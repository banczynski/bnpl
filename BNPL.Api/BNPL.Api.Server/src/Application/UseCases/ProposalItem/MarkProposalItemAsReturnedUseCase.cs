using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class MarkProposalItemAsReturnedUseCase(
        IProposalItemRepository proposalItemRepository,
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        private const int MaxReturnDays = 7;

        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalItemId, string reason)
        {
            var item = await proposalItemRepository.GetByIdAsync(proposalItemId);
            if (item is null || item.Returned)
                return Result<bool, string>.Fail("Invalid item state.");
             
            var proposal = await proposalRepository.GetByIdAsync(item.ProposalId);
            if (proposal is null || proposal.Status is not (ProposalStatus.Approved or ProposalStatus.Active))
                return Result<bool, string>.Fail("Proposal does not allow item returns.");

            if (proposal.CreatedAt < DateTime.UtcNow.AddDays(-MaxReturnDays))
                return Result<bool, string>.Fail("Return period has expired.");

            item.Returned = true;
            item.ReturnedAt = DateTime.UtcNow;
            item.ReturnReason = reason;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = userContext.GetRequiredUserId();

            await proposalItemRepository.UpdateAsync(item);
            return Result<bool, string>.Ok(true);
        }
    }

}