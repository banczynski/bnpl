using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class CreateProposalItemsUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<IEnumerable<ProposalItemDto>, string>> ExecuteAsync(Guid proposalId, CreateProposalItemsRequest request)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<IEnumerable<ProposalItemDto>, string>.Fail("Proposal not found.");

            if (!proposal.IsActive)
                return Result<IEnumerable<ProposalItemDto>, string>.Fail("Cannot add items to an inactive proposal.");

            if (proposal.Status is not ProposalStatus.Created)
                return Result<IEnumerable<ProposalItemDto>, string>.Fail("Proposal is not eligible to add an item.");

            var entities = request.Items.Select(item => item.ToEntity(proposalId, proposal.AffiliateId, userContext.GetRequiredUserId())).ToList();

            await proposalItemRepository.InsertManyAsync(entities);

            return Result<IEnumerable<ProposalItemDto>, string>.Ok(entities.ToDtoList());
        }
    }
}
