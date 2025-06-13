using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class FinancialChargesConfigurationRepository(IDbConnection connection) : GenericRepository<FinancialChargesConfiguration>(connection), IFinancialChargesConfigurationRepository
    {
        private const string InactivateByPartnerOrAffiliateSql = """
        UPDATE financial_charges_configuration
        SET is_active = FALSE,
            updated_by = @UpdatedBy,
            updated_at = @UpdatedAt
        WHERE partner_id = @PartnerId
          AND (@AffiliateId IS NULL AND affiliate_id IS NULL OR affiliate_id = @AffiliateId)
        """;

        private const string GetByAffiliateSql = "SELECT * FROM financial_charges_configuration WHERE affiliate_id = @AffiliateId LIMIT 1";
        private const string GetByPartnerSql = "SELECT * FROM financial_charges_configuration WHERE partner_id = @PartnerId AND affiliate_id IS NULL LIMIT 1";
        private const string GetAllByPartnerSql = "SELECT * FROM financial_charges_configuration WHERE partner_id = @PartnerId AND is_active = TRUE";

        public async Task<bool> InactivateByPartnerOrAffiliateAsync(Guid partnerId, Guid? affiliateId, Guid updatedBy, IDbTransaction? transaction = null)
        {
            var affectedRows = await Connection.ExecuteAsync(InactivateByPartnerOrAffiliateSql, new
            {
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                UpdatedBy = updatedBy,
                UpdatedAt = DateTime.UtcNow
            }, transaction);

            return affectedRows > 0;
        }

        public async Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(GetByAffiliateSql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(GetByPartnerSql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<FinancialChargesConfiguration>(GetAllByPartnerSql, new { PartnerId = partnerId }, transaction);
        }
    }
}