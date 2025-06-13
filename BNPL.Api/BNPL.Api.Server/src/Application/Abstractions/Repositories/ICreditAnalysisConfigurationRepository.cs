using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ICreditAnalysisConfigurationRepository : IGenericRepository<CreditAnalysisConfiguration>
    {
        Task<bool> InactivateByPartnerOrAffiliateAsync(Guid partnerId, Guid? affiliateId, Guid updatedBy, IDbTransaction? transaction = null);
        Task<CreditAnalysisConfiguration?> GetByAffiliateAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<CreditAnalysisConfiguration?> GetByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<IEnumerable<CreditAnalysisConfiguration>> GetAllByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);
    }
}