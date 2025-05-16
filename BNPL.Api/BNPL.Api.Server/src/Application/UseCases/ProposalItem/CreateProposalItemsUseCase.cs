using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class CreateProposalItemsUseCase(
    IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId, CreateProposalItemsRequest request)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (!proposal.IsActive)
                throw new InvalidOperationException("Cannot add items to an inactive proposal.");

            var existingItems = await proposalItemRepository.GetByProposalIdAsync(proposalId);
            if (existingItems.Any())
                throw new InvalidOperationException("Proposal already contains items.");

            var now = DateTime.UtcNow;
            var user = userContext.UserId;

            var entities = request.Items.Select(item => item.ToEntity(proposalId, now, user)).ToList();

            await proposalItemRepository.InsertManyAsync(entities);

            return new ServiceResult<string>("Proposal items created.");
        }
    }
}
