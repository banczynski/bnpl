using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Infrastructure.Services.Business
{
    public sealed class CreditAnalysisConfigurationService(
        ICreditAnalysisConfigurationRepository creditAnalysisConfigurationRepository
    ) : ICreditAnalysisConfigurationService
    {
        public async Task<CreditAnalysisConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId)
        {
            if (affiliateId is not null)
            {
                var config = await creditAnalysisConfigurationRepository.GetByAffiliateAsync(affiliateId.Value);
                if (config is not null)
                    return config;
            }

            var partnerConfig = await creditAnalysisConfigurationRepository.GetByPartnerAsync(partnerId);
            return partnerConfig ?? throw new InvalidOperationException("No credit analysis configuration found.");
        }
    }
}
