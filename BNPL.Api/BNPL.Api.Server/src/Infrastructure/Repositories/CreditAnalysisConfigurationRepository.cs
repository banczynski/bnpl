using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class CreditAnalysisConfigurationRepository(IDbConnection connection) : ICreditAnalysisConfigurationRepository
    {
        public async Task<CreditAnalysisConfiguration?> GetByAffiliateAsync(Guid affiliateId)
        {
            const string sql = """
            SELECT * FROM credit_analysis_configuration
            WHERE affiliate_id = @AffiliateId AND is_active = TRUE
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<CreditAnalysisConfiguration>(sql, new { AffiliateId = affiliateId });
        }

        public async Task<CreditAnalysisConfiguration?> GetByPartnerAsync(Guid partnerId)
        {
            const string sql = """
            SELECT * FROM credit_analysis_configuration
            WHERE partner_id = @PartnerId AND affiliate_id IS NULL AND is_active = TRUE
            LIMIT 1
            """;

            return await connection.QueryFirstOrDefaultAsync<CreditAnalysisConfiguration>(sql, new { PartnerId = partnerId });
        }

        public async Task<IEnumerable<CreditAnalysisConfiguration>> GetAllByPartnerAsync(Guid partnerId)
        {
            const string sql = """
            SELECT * FROM credit_analysis_configuration
            WHERE partner_id = @PartnerId AND is_active = TRUE
            """;

            return await connection.QueryAsync<CreditAnalysisConfiguration>(sql, new { PartnerId = partnerId });
        }

        public async Task InsertAsync(CreditAnalysisConfiguration config)
            => await connection.InsertAsync(config);

        public async Task UpdateAsync(CreditAnalysisConfiguration config)
            => await connection.UpdateAsync(config);

        public async Task InactivateAsync(Guid partnerId, Guid? affiliateId, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE credit_analysis_configuration
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
    }
}
