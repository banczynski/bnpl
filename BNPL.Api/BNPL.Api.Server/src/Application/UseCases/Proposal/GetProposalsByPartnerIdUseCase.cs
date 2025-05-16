using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalsByPartnerIdUseCase(IProposalRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<ProposalDto>>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var items = await repository.GetByPartnerIdAsync(partnerId, onlyActive);
            return new ServiceResult<IEnumerable<ProposalDto>>(items.Select(p => p.ToDto()));
        }
    }
}
