using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerRepository(IDbConnection connection) : ICustomerRepository
    {
        public IDbConnection Connection => connection;

        public async Task InsertAsync(Customer customer, IDbTransaction? transaction = null)
            => await connection.InsertAsync(customer, transaction);

        public async Task UpdateAsync(Customer customer, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(customer, transaction);

        public async Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE customer
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE code = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt }, transaction);
        }

        public async Task<Customer?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM customer WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id }, transaction);
        }

        public async Task<Guid?> GetIdByTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT code FROM customer WHERE tax_id = @TaxId LIMIT 1";
            return await connection.ExecuteScalarAsync<Guid>(sql, new { TaxId = taxId }, transaction);
        }

        public async Task<IEnumerable<Customer?>> GetByTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM customer WHERE tax_id = @TaxId";
            return await connection.QueryAsync<Customer>(sql, new { TaxId = taxId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM customer
            WHERE affiliate_id = @AffiliateId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Customer>(sql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM customer
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Customer>(sql, new { PartnerId = partnerId }, transaction);
        }
    }
}
