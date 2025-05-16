using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class CreditAnalysisConfigurationService(
        ICreditAnalysisConfigurationRepository repository
    ) : ICreditAnalysisConfigurationService
    {
        public async Task<CreditAnalysisConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId)
        {
            if (affiliateId is not null)
            {
                var config = await repository.GetByAffiliateAsync(affiliateId.Value);
                if (config is not null)
                    return config;
            }

            var partnerConfig = await repository.GetByPartnerAsync(partnerId);
            return partnerConfig ?? throw new InvalidOperationException("No credit analysis configuration found.");
        }
    }
}
