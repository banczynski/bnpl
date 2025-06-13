using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IFinancialChargesConfigurationRepository : IGenericRepository<FinancialChargesConfiguration>
    {
        Task<bool> InactivateByPartnerOrAffiliateAsync(Guid partnerId, Guid? affiliateId, Guid updatedBy, IDbTransaction? transaction = null);
        Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);
    }
}