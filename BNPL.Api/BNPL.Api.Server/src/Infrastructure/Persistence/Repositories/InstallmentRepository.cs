using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class InstallmentRepository(IDbConnection connection) : IInstallmentRepository
    {
        public async Task UpdateAsync(Installment installment, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(installment, transaction);

        public async Task InsertManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null)
        {
            foreach (var i in installments)
                await connection.InsertAsync(i, transaction);
        }

        public async Task UpdateManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null)
        {
            foreach (var i in installments)
                await connection.UpdateAsync(i, transaction);
        }

        public async Task<Installment?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM installment WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Installment>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE proposal_id = @ProposalId AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE customer_id = @CustomerId
            AND status = 0
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { CustomerId = customerId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetAllOpenByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE customer_id = @CustomerId
              AND is_active = true
              AND status IN (@Pending, @Overdue)
            """;

            return await connection.QueryAsync<Installment>(sql, new
            {
                CustomerId = customerId,
                Pending = InstallmentStatus.Pending,
                Overdue = InstallmentStatus.Overdue
            }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingDueInDaysAsync(int days, IDbTransaction? transaction = null)
        {
            var limitDate = DateTime.UtcNow.Date.AddDays(days);

            const string sql = """
            SELECT * FROM installment
            WHERE status = 0
            AND due_date <= @LimitDate
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { LimitDate = limitDate }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetByInvoiceIdAsync(Guid invoiceId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE invoice_id = @InvoiceId
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { InvoiceId = invoiceId }, transaction);
        }

        public async Task<IEnumerable<Installment>> GetPendingByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE id = ANY(@Ids)
            AND status = 0
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { Ids = ids.ToArray() }, transaction);
        }
    }

}
