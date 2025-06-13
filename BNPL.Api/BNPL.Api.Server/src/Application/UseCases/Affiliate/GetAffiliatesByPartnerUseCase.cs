using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliatesByPartnerUseCase(IAffiliateRepository affiliateRepository)
    {
        public async Task<Result<IEnumerable<AffiliateDto>, Error>> ExecuteAsync(Guid partnerId)
        {
            var affiliates = await affiliateRepository.GetActivesByPartnerIdAsync(partnerId);
            return Result<IEnumerable<AffiliateDto>, Error>.Ok(affiliates.Select(a => a.ToDto()));
        }
    }
}