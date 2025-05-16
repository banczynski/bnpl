using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalsByAffiliateIdUseCase(IProposalRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<ProposalDto>>> ExecuteAsync(Guid affiliateId, bool onlyActive = true)
        {
            var items = await repository.GetByAffiliateIdAsync(affiliateId, onlyActive);
            return new ServiceResult<IEnumerable<ProposalDto>>(items.Select(p => p.ToDto()));
        }
    }
}
