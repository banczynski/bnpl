using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalRepository(IDbConnection connection) : IProposalRepository
    {
        public async Task InsertAsync(Proposal proposal, IDbTransaction? transaction = null)
            => await connection.InsertAsync(proposal, transaction);
        
        public async Task UpdateAsync(Proposal proposal, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(proposal, transaction);

        public async Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE proposal
            SET is_active = FALSE,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE code = @Id
            """;

            await connection.ExecuteAsync(sql, new { Id = id, UpdatedBy = updatedBy, UpdatedAt = updatedAt }, transaction);
        }

        public async Task UpdateManyAsync(IEnumerable<Proposal> proposals, IDbTransaction? transaction = null)
        {
            foreach (var p in proposals)
                await connection.UpdateAsync(p, transaction);
        }

        public async Task<Proposal?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM proposal WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Proposal>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM proposal
            WHERE id = ANY(@Ids)
            """;

            return await connection.QueryAsync<Proposal>(sql, new { Ids = ids.ToArray() }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync(IDbTransaction? transaction = null)
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

            return await connection.QueryAsync<Proposal>(sql, new { Status = ProposalStatus.Signed }, transaction);
        }

        public async Task<Proposal?> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : ""
            + """ LIMIT 1""");

            return await connection.QueryFirstOrDefaultAsync<Proposal>(sql, new { CustomerId = customerId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetListByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE customer_id = @CustomerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { CustomerId = customerId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE affiliate_id = @AffiliateId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null)
        {
            var sql = """
            SELECT * FROM proposal
            WHERE partner_id = @PartnerId
            """ + (onlyActive ? " AND is_active = TRUE" : "");

            return await connection.QueryAsync<Proposal>(sql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetPendingBeforeDateAsync(DateTime cutoff, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM proposal
            WHERE status IN (@Created, @AwaitingSignature)
            AND created_at < @Cutoff
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Proposal>(sql, new
            {
                ProposalStatus.Created,
                ProposalStatus.AwaitingSignature,
                Cutoff = cutoff
            }, transaction);
        }

        public async Task<bool> ExistsActiveByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT 1
            FROM proposal
            WHERE partner_id = @PartnerId
              AND is_active = TRUE
              AND status = @Active)
            LIMIT 1
            """;

            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                sql,
                new
                {
                    PartnerId = partnerId,
                    Active = ProposalStatus.Active
                },
                transaction
            );

            return result.HasValue;
        }

        public async Task<bool> ExistsActiveByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT 1
            FROM proposal
            WHERE affiliate_id = @AffiliateId
              AND is_active = TRUE
              AND status = @Active)
            LIMIT 1
            """;

            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                sql,
                new
                {
                    AffiliateId = affiliateId,
                    Active = ProposalStatus.Active
                },
                transaction
            );

            return result.HasValue;
        }

        public async Task<bool> ExistsActiveByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT 1
            FROM proposal
            WHERE customer_id = @CustomerId
              AND is_active = TRUE
              AND status = @Active)
            LIMIT 1
            """;

            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                sql,
                new
                {
                    CustomerId = customerId,
                    Active = ProposalStatus.Active
                },
                transaction
            );

            return result.HasValue;
        }
    }
}
