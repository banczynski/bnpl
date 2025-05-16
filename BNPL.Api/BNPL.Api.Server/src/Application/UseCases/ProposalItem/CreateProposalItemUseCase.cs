using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class CreateProposalItemUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId, CreateProposalItemRequest request)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (!proposal.IsActive)
                throw new InvalidOperationException("Cannot add item to an inactive proposal.");

            var now = DateTime.UtcNow;
            var entity = request.ToEntity(proposalId, now, userContext.UserId);

            await proposalItemRepository.InsertAsync(entity);

            return new ServiceResult<string>("Proposal item created.");
        }
    }
}
