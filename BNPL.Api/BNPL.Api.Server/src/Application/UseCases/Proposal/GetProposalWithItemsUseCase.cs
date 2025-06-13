using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalWithItemsUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository
    )
    {
        public async Task<Result<ProposalWithItemsDto, string>> ExecuteAsync(Guid id)
        {
            var proposal = await proposalRepository.GetByIdAsync(id);
            if (proposal is null)
                return Result<ProposalWithItemsDto, string>.Fail("Proposal not found.");

            var items = await proposalItemRepository.GetByProposalIdAsync(id);

            var dto = new ProposalWithItemsDto
            {
                Proposal = proposal.ToDto(),
                Items = [.. items.Select(x => x.ToDto())]
            };

            return Result<ProposalWithItemsDto, string>.Ok(dto);
        }
    }
}
