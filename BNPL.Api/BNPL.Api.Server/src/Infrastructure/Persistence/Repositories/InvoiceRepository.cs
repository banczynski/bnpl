using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class InvoiceRepository(IDbConnection connection) : IInvoiceRepository
    {
        public async Task InsertAsync(Invoice invoice, IDbTransaction? transaction = null)
            => await connection.InsertAsync(invoice, transaction);

        public async Task UpdateAsync(Invoice invoice, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(invoice, transaction);

        public async Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE invoice
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE code = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt }, transaction);
        }

        public async Task UpdateManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null)
        {
            foreach (var invoice in invoices)
                await connection.UpdateAsync(invoice, transaction);
        }

        public async Task InsertManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null)
        {
            foreach (var invoice in invoices)
                await connection.InsertAsync(invoice, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM invoice
            WHERE proposal_id = @ProposalId
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Invoice>(sql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null)
        {
            if (!ids.Any())
                return [];

            const string sql = """
            SELECT * FROM invoice
            WHERE id = ANY(@Ids)
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Invoice>(sql, new { Ids = ids.ToArray() }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM invoice
            WHERE status = 0
            AND due_date < @ReferenceDate
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Invoice>(sql, new { ReferenceDate = referenceDate.Date }, transaction);
        }

        public async Task<Invoice?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM invoice WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Invoice>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM invoice
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Invoice>(sql, new { CustomerId = customerId }, transaction);
        }

        public async Task<Invoice?> GetByCustomerAndDueDateAsync(Guid customerId, DateTime dueDate, IDbTransaction? transaction = null)
        {
            const string sql = """
                SELECT *
                FROM invoice
                WHERE customer_id = @CustomerId
                  AND due_date = @DueDate
                  AND is_active = TRUE
                LIMIT 1;
            """;

            return await connection.QueryFirstOrDefaultAsync<Invoice>(sql, new { CustomerId = customerId, DueDate = dueDate }, transaction);
        }
    }
}
