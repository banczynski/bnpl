using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalWithItemsUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository
    )
    {
        public async Task<ServiceResult<ProposalWithItemsDto>> ExecuteAsync(Guid id)
        {
            var proposal = await proposalRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Proposal not found.");

            var items = await proposalItemRepository.GetByProposalIdAsync(id);

            var dto = new ProposalWithItemsDto
            {
                Proposal = proposal.ToDto(),
                Items = [.. items.Select(x => x.ToDto())]
            };

            return new ServiceResult<ProposalWithItemsDto>(dto);
        }
    }
}
