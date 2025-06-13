using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class InvoiceRepository(IDbConnection connection) : GenericRepository<Invoice>(connection), IInvoiceRepository
    {
        private const string GetByProposalIdSql = "SELECT * FROM invoice WHERE proposal_id = @ProposalId AND is_active = TRUE";
        private const string GetByIdsSql = "SELECT * FROM invoice WHERE code = ANY(@Codes) AND is_active = TRUE";
        private const string GetOverduePendingSql = "SELECT * FROM invoice WHERE status = 0 AND due_date < @ReferenceDate AND is_active = TRUE";
        private const string GetActivesByCustomerSql = "SELECT * FROM invoice WHERE customer_id = @CustomerId AND is_active = TRUE";
        private const string GetByCustomerAndDueDateSql = "SELECT * FROM invoice WHERE customer_id = @CustomerId AND due_date = @DueDate AND is_active = TRUE LIMIT 1";

        public async Task UpdateManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null)
        {
            foreach (var invoice in invoices) { await base.UpdateAsync(invoice, transaction); }
        }

        public async Task InsertManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null)
        {
            foreach (var invoice in invoices) { await base.InsertAsync(invoice, transaction); }
        }

        public async Task<IEnumerable<Invoice>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Invoice>(GetByProposalIdSql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> codes, IDbTransaction? transaction = null)
        {
            if (!codes.Any()) return [];
            return await Connection.QueryAsync<Invoice>(GetByIdsSql, new { Codes = codes.ToArray() }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Invoice>(GetOverduePendingSql, new { ReferenceDate = referenceDate.Date }, transaction);
        }

        public async Task<IEnumerable<Invoice>> GetActivesByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Invoice>(GetActivesByCustomerSql, new { CustomerId = customerId }, transaction);
        }

        public async Task<Invoice?> GetByCustomerAndDueDateAsync(Guid customerId, DateTime dueDate, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<Invoice>(GetByCustomerAndDueDateSql, new { CustomerId = customerId, DueDate = dueDate }, transaction);
        }
    }
}