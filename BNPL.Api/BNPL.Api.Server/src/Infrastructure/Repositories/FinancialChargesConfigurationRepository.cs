using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class FinancialChargesConfigurationRepository(IDbConnection connection) : IFinancialChargesConfigurationRepository
    {
        public async Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE affiliate_id = @AffiliateId
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(sql, new { AffiliateId = affiliateId });
        }

        public async Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE partner_id = @PartnerId AND affiliate_id IS NULL
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(sql, new { PartnerId = partnerId });
        }

        public async Task InsertAsync(FinancialChargesConfiguration config)
            => await connection.InsertAsync(config);

        public async Task UpdateAsync(FinancialChargesConfiguration config)
            => await connection.UpdateAsync(config);

        public async Task InactivateAsync(Guid partnerId, Guid? affiliateId, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE financial_charges_configuration
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE partner_id = @PartnerId
            AND (@AffiliateId IS NULL OR affiliate_id = @AffiliateId)
            """;

            await connection.ExecuteAsync(sql, new
            {
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                UpdatedBy = updatedBy,
                UpdatedAt = updatedAt
            });
        }

        public async Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE partner_id = @PartnerId AND is_active = TRUE
            """;

            return await connection.QueryAsync<FinancialChargesConfiguration>(sql, new { PartnerId = partnerId });
        }
    }
}
