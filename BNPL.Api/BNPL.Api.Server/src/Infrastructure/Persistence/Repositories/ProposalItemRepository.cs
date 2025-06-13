using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalItemRepository(IDbConnection connection) : GenericRepository<ProposalItem>(connection), IProposalItemRepository
    {
        private const string GetByProposalIdSql = "SELECT * FROM proposal_item WHERE proposal_id = @ProposalId";
        private const string MarkAllAsReturnedSql = """
            UPDATE proposal_item
            SET returned = true,
                return_reason = @Reason,
                returned_at = NOW()
            WHERE proposal_id = @ProposalId 
            """;

        public async Task InsertManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null)
        {
            foreach (var item in items)
            {
                await base.InsertAsync(item, transaction);
            }
        }

        public async Task UpdateManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null)
        {
            foreach (var item in items)
            {
                await base.UpdateAsync(item, transaction);
            }
        }

        public async Task<IEnumerable<ProposalItem>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<ProposalItem>(GetByProposalIdSql, new { ProposalId = proposalId }, transaction);
        }

        public async Task MarkAllItemsAsReturnedByProposalIdAsync(Guid proposalId, string reason, IDbTransaction? transaction = null)
        {
            await Connection.ExecuteAsync(MarkAllAsReturnedSql, new { ProposalId = proposalId, Reason = reason }, transaction);
        }
    }
}