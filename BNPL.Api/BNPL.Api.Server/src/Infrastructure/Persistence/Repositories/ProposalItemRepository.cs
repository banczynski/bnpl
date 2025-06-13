using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalItemRepository(IDbConnection connection) : IProposalItemRepository
    {
        public async Task InsertAsync(ProposalItem proposalItem, IDbTransaction? transaction = null)
        {
            await connection.InsertAsync(proposalItem, transaction);
        }

        public async Task InsertManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null)
        {
            foreach (var item in items)
                await connection.InsertAsync(item, transaction);
        }

        public async Task UpdateManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null)
        {
            foreach (var item in items)
                await connection.UpdateAsync(item, transaction);
        }

        public async Task<IEnumerable<ProposalItem>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM proposal_item WHERE proposal_id = @ProposalId";
            return await connection.QueryAsync<ProposalItem>(sql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<ProposalItem?> GetByIdAsync(Guid proposalItemId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM proposal_item WHERE code = @ProposalItemId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<ProposalItem>(sql, new { ProposalItemId = proposalItemId }, transaction);
        }

        public async Task UpdateAsync(ProposalItem item, IDbTransaction? transaction = null)
        {
            await connection.UpdateAsync(item, transaction);
        }

        public async Task MarkAllItemsAsReturnedByProposalIdAsync(Guid proposalId, string reason, IDbTransaction? transaction = null)
        {
            const string sql = """
            UPDATE proposal_item
            SET returned = true,
                return_reason = @Reason,
                returned_at = NOW()
            WHERE proposal_id = @ProposalId 
            """;

            await connection.ExecuteAsync(sql, new
            {
                ProposalId = proposalId,
                Reason = reason
            }, transaction);
        }
    }
}
