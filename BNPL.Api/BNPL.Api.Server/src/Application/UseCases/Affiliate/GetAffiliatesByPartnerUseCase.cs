using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliatesByPartnerUseCase(IAffiliateRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<AffiliateDto>>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var affiliates = await repository.GetByPartnerIdAsync(partnerId, onlyActive);
            return new ServiceResult<IEnumerable<AffiliateDto>>(affiliates.Select(a => a.ToDto()));
        }
    }
}
