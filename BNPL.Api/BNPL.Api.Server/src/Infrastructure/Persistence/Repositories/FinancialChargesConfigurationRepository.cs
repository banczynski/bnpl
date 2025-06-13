using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class FinancialChargesConfigurationRepository(IDbConnection connection) : IFinancialChargesConfigurationRepository
    {
        public async Task InsertAsync(FinancialChargesConfiguration config, IDbTransaction? transaction = null)
            => await connection.InsertAsync(config, transaction);

        public async Task UpdateAsync(FinancialChargesConfiguration config, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(config, transaction);

        public async Task InactivateAsync(Guid partnerId, Guid? affiliateId, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null)
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
            }, transaction);
        }

        public async Task<FinancialChargesConfiguration?> GetByAffiliateAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE affiliate_id = @AffiliateId
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(sql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<FinancialChargesConfiguration?> GetByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE partner_id = @PartnerId AND affiliate_id IS NULL
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<FinancialChargesConfiguration>(sql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<FinancialChargesConfiguration>> GetAllByPartnerAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM financial_charges_configuration
            WHERE partner_id = @PartnerId AND is_active = TRUE
            """;

            return await connection.QueryAsync<FinancialChargesConfiguration>(sql, new { PartnerId = partnerId }, transaction);
        }
    }
}
