using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Infrastructure.Services.Business
{
    public sealed class FinancialChargesConfigurationService(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository
    ) : IFinancialChargesConfigurationService
    {
        public async Task<FinancialChargesConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId)
        {
            if (affiliateId is not null)
            {
                var affiliateConfig = await financialChargesConfigurationRepository.GetByAffiliateAsync(affiliateId.Value);
                if (affiliateConfig is not null)
                    return affiliateConfig;
            }

            var partnerConfig = await financialChargesConfigurationRepository.GetByPartnerAsync(partnerId);
            return partnerConfig ?? throw new InvalidOperationException("No financial charges configuration found.");
        }
    }
}
