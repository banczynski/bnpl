using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliatesByPartnerUseCase(IAffiliateRepository affiliateRepository)
    {
        public async Task<Result<IEnumerable<AffiliateDto>, string>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var affiliates = await affiliateRepository.GetByPartnerIdAsync(partnerId, onlyActive);
            return Result<IEnumerable<AffiliateDto>, string>.Ok(affiliates.Select(a => a.ToDto()));
        }
    }
}
