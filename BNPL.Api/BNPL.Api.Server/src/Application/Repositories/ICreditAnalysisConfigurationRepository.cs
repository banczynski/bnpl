using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface ICreditAnalysisConfigurationRepository
    {
        Task<CreditAnalysisConfiguration?> GetByAffiliateAsync(Guid affiliateId);
        Task<CreditAnalysisConfiguration?> GetByPartnerAsync(Guid partnerId);
        Task<IEnumerable<CreditAnalysisConfiguration>> GetAllByPartnerAsync(Guid partnerId);
        Task InsertAsync(CreditAnalysisConfiguration config);
        Task UpdateAsync(CreditAnalysisConfiguration config);
        Task InactivateAsync(Guid partnerId, Guid? affiliateId, string updatedBy, DateTime updatedAt);
    }
}
