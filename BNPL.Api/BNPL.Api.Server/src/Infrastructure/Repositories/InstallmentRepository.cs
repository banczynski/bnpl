using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class InstallmentRepository(IDbConnection connection) : IInstallmentRepository
    {
        public async Task InsertManyAsync(IEnumerable<Installment> installments)
        {
            foreach (var i in installments)
                await connection.InsertAsync(i);
        }

        public async Task<Installment?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Installment>(id);

        public async Task<IEnumerable<Installment>> GetByProposalIdAsync(Guid proposalId)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE proposal_id = @ProposalId AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { ProposalId = proposalId });
        }

        public async Task<IEnumerable<Installment>> GetPendingByCustomerIdAsync(Guid customerId)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE customer_id = @CustomerId
            AND status = 0
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Installment>> GetPendingDueInDaysAsync(int days)
        {
            var limitDate = DateTime.UtcNow.Date.AddDays(days);

            const string sql = """
            SELECT * FROM installment
            WHERE status = 0
            AND due_date <= @LimitDate
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { LimitDate = limitDate });
        }

        public async Task<IEnumerable<Installment>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE invoice_id = @InvoiceId
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { InvoiceId = invoiceId });
        }

        public async Task<IEnumerable<Installment>> GetPendingByIdsAsync(IEnumerable<Guid> ids)
        {
            const string sql = """
            SELECT * FROM installment
            WHERE id = ANY(@Ids)
            AND status = 0
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Installment>(sql, new { Ids = ids.ToArray() });
        }

        public async Task UpdateAsync(Installment installment)
            => await connection.UpdateAsync(installment);
    }

}
