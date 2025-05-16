using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IFinancialChargesConfigurationRepository
    {
        Task InsertAsync(FinancialChargesConfiguration config);
        Task UpdateAsync(FinancialChargesConfiguration config);
        Task InactivateAsync(Guid partnerId, Guid? affiliateId, string updatedBy, DateTime updatedAt);
        Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId);
        Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId);
        Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId);

    }
}
