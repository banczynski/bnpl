using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class CreateProposalItemUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<ProposalItemDto, string>> ExecuteAsync(Guid proposalId, CreateProposalItemRequest request)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<ProposalItemDto, string>.Fail("Proposal not found.");

            if (!proposal.IsActive)
                return Result<ProposalItemDto, string>.Fail("Cannot add item to an inactive proposal.");

            if (proposal.Status is not ProposalStatus.Created)
                return Result<ProposalItemDto, string>.Fail("Proposal is not eligible to add an item.");

            var entity = request.ToEntity(proposalId, proposal.AffiliateId, userContext.GetRequiredUserId());

            await proposalItemRepository.InsertAsync(entity);

            return Result<ProposalItemDto, string>.Ok(entity.ToDto());
        }
    }
}
