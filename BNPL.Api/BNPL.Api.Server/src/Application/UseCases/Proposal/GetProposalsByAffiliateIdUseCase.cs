using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalsByAffiliateIdUseCase(IProposalRepository proposalRepository)
    {
        public async Task<Result<IEnumerable<ProposalDto>, Error>> ExecuteAsync(Guid affiliateId)
        {
            var items = await proposalRepository.GetActivesByAffiliateIdAsync(affiliateId);

            return items is null || !items.Any()
                ? Result<IEnumerable<ProposalDto>, Error>.Fail(DomainErrors.Proposal.NotFoundForCriteria)
                : Result<IEnumerable<ProposalDto>, Error>.Ok(items.ToDtoList());
        }
    }
}