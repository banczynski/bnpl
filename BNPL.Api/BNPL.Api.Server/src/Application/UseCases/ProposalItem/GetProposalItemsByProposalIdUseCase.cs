using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class GetProposalItemsByProposalIdUseCase(
        IProposalItemRepository repository
    )
    {
        public async Task<ServiceResult<List<ProposalItemDto>>> ExecuteAsync(Guid proposalId)
        {
            var items = await repository.GetByProposalIdAsync(proposalId);
            var dtos = items.Select(i => i.ToDto()).ToList();
            return new ServiceResult<List<ProposalItemDto>>(dtos);
        }
    }
}
