using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class GetProposalItemsByProposalIdUseCase(
        IProposalItemRepository proposalItemRepository
    )
    {
        public async Task<Result<IEnumerable<ProposalItemDto>, Error>> ExecuteAsync(Guid proposalId)
        {
            var items = await proposalItemRepository.GetByProposalIdAsync(proposalId);
            return Result<IEnumerable<ProposalItemDto>, Error>.Ok(items.ToDtoList());
        }
    }
}