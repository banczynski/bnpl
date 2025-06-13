using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalsByPartnerIdUseCase(IProposalRepository proposalRepository)
    {
        public async Task<Result<IEnumerable<ProposalDto>, string>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var items = await proposalRepository.GetByPartnerIdAsync(partnerId, onlyActive);

            return items is null || !items.Any()
                ? Result<IEnumerable<ProposalDto>, string>.Fail("No proposals found for this partner.")
                : Result<IEnumerable<ProposalDto>, string>.Ok(items.ToDtoList());
        }
    }
}
