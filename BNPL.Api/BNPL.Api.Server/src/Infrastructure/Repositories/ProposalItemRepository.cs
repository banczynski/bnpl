using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class ProposalItemRepository(IDbConnection connection) : IProposalItemRepository
    {
        public async Task InsertAsync(ProposalItem proposalItem)
        {
            await connection.InsertAsync(proposalItem);
        }

        public async Task InsertManyAsync(IEnumerable<ProposalItem> items)
        {
            foreach (var item in items)
                await connection.InsertAsync(item);
        }

        public async Task<IEnumerable<ProposalItem>> GetByProposalIdAsync(Guid proposalId)
        {
            const string sql = "SELECT * FROM proposal_item WHERE proposal_id = @ProposalId";
            return await connection.QueryAsync<ProposalItem>(sql, new { ProposalId = proposalId });
        }

        public async Task MarkAsReturnedAsync(Guid proposalId, Guid productId, string reason)
        {
            const string sql = """
            UPDATE proposal_item
            SET returned = true,
                return_reason = @Reason,
                returned_at = NOW()
            WHERE proposal_id = @ProposalId AND product_id = @ProductId
            """;

            await connection.ExecuteAsync(sql, new
            {
                ProposalId = proposalId,
                ProductId = productId,
                Reason = reason
            });
        }
    }
}
