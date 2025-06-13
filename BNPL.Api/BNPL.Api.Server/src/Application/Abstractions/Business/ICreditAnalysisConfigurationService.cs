using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Abstractions.Business
{
    public interface ICreditAnalysisConfigurationService
    {
        Task<CreditAnalysisConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId);
    }
}
