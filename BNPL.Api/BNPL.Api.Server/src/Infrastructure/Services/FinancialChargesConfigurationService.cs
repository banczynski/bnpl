using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class FinancialChargesConfigurationService(
        IFinancialChargesConfigurationRepository repository
    ) : IFinancialChargesConfigurationService
    {
        public async Task<FinancialChargesConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId)
        {
            if (affiliateId is not null)
            {
                var affiliateConfig = await repository.GetByAffiliateAsync(affiliateId.Value);
                if (affiliateConfig is not null)
                    return affiliateConfig;
            }

            var partnerConfig = await repository.GetByPartnerAsync(partnerId);
            return partnerConfig ?? throw new InvalidOperationException("No financial charges configuration found.");
        }
    }
}
