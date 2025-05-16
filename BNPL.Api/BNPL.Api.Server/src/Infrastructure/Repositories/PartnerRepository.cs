using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class PartnerRepository(IDbConnection connection) : IPartnerRepository
    {
        public async Task InsertAsync(Partner partner)
            => await connection.InsertAsync(partner);

        public async Task UpdateAsync(Partner partner)
            => await connection.UpdateAsync(partner);

        public async Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE partner
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt });
        }

        public async Task<Partner?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Partner>(id);

        public async Task<IEnumerable<Partner>> GetAllAsync(bool onlyActive = true)
        {
            var sql = "SELECT * FROM partner" + (onlyActive ? " WHERE is_active = TRUE" : "");
            return await connection.QueryAsync<Partner>(sql);
        }
    }
}
