using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class CustomerRepository(IDbConnection connection) : ICustomerRepository
    {
        public async Task InsertAsync(Customer customer)
            => await connection.InsertAsync(customer);

        public async Task UpdateAsync(Customer customer)
            => await connection.UpdateAsync(customer);

        public async Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE customer
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt });
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Customer>(id);

        public async Task<IEnumerable<Customer>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM customer
            WHERE affiliate_id = @AffiliateId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Customer>(sql, new { AffiliateId = affiliateId });
        }

        public async Task<IEnumerable<Customer>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM customer
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Customer>(sql, new { PartnerId = partnerId });
        }
    }
}
