using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IProposalItemRepository : IGenericRepository<ProposalItem>
    {
        Task InsertManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null);
        Task UpdateManyAsync(IEnumerable<ProposalItem> items, IDbTransaction? transaction = null);
        Task<IEnumerable<ProposalItem>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null);
        Task MarkAllItemsAsReturnedByProposalIdAsync(Guid proposalId, string reason, IDbTransaction? transaction = null);
    }
}