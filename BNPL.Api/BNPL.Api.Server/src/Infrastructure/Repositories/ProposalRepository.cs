using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class ProposalRepository(IDbConnection connection) : IProposalRepository
    {
        public async Task InsertAsync(Proposal proposal)
            => await connection.InsertAsync(proposal);

        public async Task UpdateAsync(Proposal proposal)
            => await connection.UpdateAsync(proposal);

        public async Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt)
        {
            const string sql = """
            UPDATE proposal
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt });
        }

        public async Task<Proposal?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Proposal>(id);

        public async Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            const string sql = """
            SELECT * FROM proposal
            WHERE id = ANY(@Ids)
            """;

            return await connection.QueryAsync<Proposal>(sql, new { Ids = ids.ToArray() });
        }

        public async Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync()
        {
            const string sql = """
            SELECT * FROM proposal
            WHERE status = @Status
            AND is_active = TRUE
            AND id NOT IN (
                SELECT UNNEST(proposal_ids) FROM invoice
                WHERE is_active = TRUE
            )
            """;

            return await connection.QueryAsync<Proposal>(sql, new { Status = ProposalStatus.Signed });
        }

        public async Task<Proposal?> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : ""
            + """ LIMIT 1""");

            return await connection.QueryFirstOrDefaultAsync<Proposal>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Proposal>> GetListByCustomerIdAsync(Guid customerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Proposal>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE affiliate_id = @AffiliateId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { AffiliateId = affiliateId });
        }

        public async Task<IEnumerable<Proposal>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { PartnerId = partnerId });
        }
    }
}
