using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IFinancialChargesConfigurationRepository
    {
        Task InsertAsync(FinancialChargesConfiguration config, IDbTransaction? transaction = null);
        Task UpdateAsync(FinancialChargesConfiguration config, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid partnerId, Guid? affiliateId, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null);
        Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null);

    }
}
