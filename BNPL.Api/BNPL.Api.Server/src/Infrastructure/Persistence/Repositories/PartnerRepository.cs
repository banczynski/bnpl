using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class PartnerRepository(IDbConnection connection) : IPartnerRepository
    {
        public async Task InsertAsync(Partner partner, IDbTransaction? transaction = null)
            => await connection.InsertAsync(partner, transaction);

        public async Task UpdateAsync(Partner partner, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(partner, transaction);

        public async Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE partner
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE code = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt }, transaction);
        }

        public async Task<Partner?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM partner WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Partner>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Partner>> GetAllAsync(bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = "SELECT * FROM partner" + (onlyActive ? " WHERE is_active = TRUE" : "");
            return await connection.QueryAsync<Partner>(sql, transaction);
        }
    }
}
