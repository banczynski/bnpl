using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class AffiliateRepository(IDbConnection connection) : IAffiliateRepository
    {
        public async Task InsertAsync(Affiliate affiliate)
            => await connection.InsertAsync(affiliate);

        public async Task UpdateAsync(Affiliate affiliate)
            => await connection.UpdateAsync(affiliate);

        public async Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE affiliate
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt });
        }

        public async Task<Affiliate?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Affiliate>(id);

        public async Task<IEnumerable<Affiliate>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM affiliate
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Affiliate>(sql, new { PartnerId = partnerId });
        }
    }
}
