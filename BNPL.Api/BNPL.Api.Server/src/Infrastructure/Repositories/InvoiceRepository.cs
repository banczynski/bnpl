using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class InvoiceRepository(IDbConnection connection) : IInvoiceRepository
    {
        public async Task InsertAsync(Invoice invoice)
            => await connection.InsertAsync(invoice);

        public async Task UpdateAsync(Invoice invoice)
            => await connection.UpdateAsync(invoice);

        public async Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE invoice
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt });
        }

        public async Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            if (!ids.Any())
                return [];

            const string sql = """
            SELECT * FROM invoice
            WHERE id = ANY(@Ids)
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Invoice>(sql, new { Ids = ids.ToArray() });
        }

        public async Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate)
        {
            const string sql = """
            SELECT * FROM invoice
            WHERE status = 0
            AND due_date < @ReferenceDate
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Invoice>(sql, new { ReferenceDate = referenceDate.Date });
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Invoice>(id);

        public async Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM invoice
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Invoice>(sql, new { CustomerId = customerId });
        }
    }
}
