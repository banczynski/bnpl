using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class AffiliateRepository(IDbConnection connection) : IAffiliateRepository
    {
        public async Task InsertAsync(Affiliate affiliate, IDbTransaction? transaction = null)
            => await connection.InsertAsync(affiliate, transaction);

        public async Task UpdateAsync(Affiliate affiliate, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(affiliate, transaction);

        public async Task InactivateAsync(Guid id, Guid updatedBy, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE affiliate
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE code = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = DateTime.UtcNow }, transaction);
        }

        public async Task<Guid?> GetPartnerIdByAffiliateIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT partner_id FROM affiliate WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Guid>(sql, new { Id = id }, transaction);
        }

        public async Task<Affiliate?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM affiliate WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Affiliate>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Affiliate>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM affiliate
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Affiliate>(sql, new { PartnerId = partnerId }, transaction);
        }
    }
}
