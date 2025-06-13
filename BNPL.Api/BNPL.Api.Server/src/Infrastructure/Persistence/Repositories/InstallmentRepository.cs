using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class InstallmentRepository(IDbConnection connection) : GenericRepository<Installment>(connection), IInstallmentRepository
    {
        private const string GetByProposalIdSql = "SELECT * FROM installment WHERE proposal_id = @ProposalId AND is_active = TRUE";
        private const string GetPendingByCustomerSql = "SELECT * FROM installment WHERE customer_id = @CustomerId AND status = 0 AND is_active = TRUE";
        private const string GetAllOpenByCustomerSql = "SELECT * FROM installment WHERE customer_id = @CustomerId AND is_active = true AND status IN (@Pending, @Overdue)";
        private const string GetPendingDueInDaysSql = "SELECT * FROM installment WHERE status = 0 AND due_date <= @LimitDate AND is_active = TRUE";
        private const string GetByInvoiceIdSql = "SELECT * FROM installment WHERE invoice_id = @InvoiceId AND is_active = TRUE";
        private const string GetPendingByIdsSql = "SELECT * FROM installment WHERE id = ANY(@Ids) AND status = 0 AND is_active = TRUE";

        public async Task InsertManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null)
        {
            foreach (var i in installments) { await base.InsertAsync(i, transaction); }
        }

        public async Task UpdateManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null)
        {
            foreach (var i in installments) { await base.UpdateAsync(i, transaction); }
        }

        public async Task<IEnumerable<Installment>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Installment>(GetByProposalIdSql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Installment>(GetPendingByCustomerSql, new { CustomerId = customerId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetAllOpenByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Installment>(GetAllOpenByCustomerSql, new { CustomerId = customerId, Pending = InstallmentStatus.Pending, Overdue = InstallmentStatus.Overdue }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingDueInDaysAsync(int days, IDbTransaction? transaction = null)
        {
            var limitDate = DateTime.UtcNow.Date.AddDays(days);
            return await Connection.QueryAsync<Installment>(GetPendingDueInDaysSql, new { LimitDate = limitDate }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetByInvoiceIdAsync(Guid invoiceId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Installment>(GetByInvoiceIdSql, new { InvoiceId = invoiceId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Installment>(GetPendingByIdsSql, new { Ids = ids.ToArray() }, transaction);
        }
    }
}