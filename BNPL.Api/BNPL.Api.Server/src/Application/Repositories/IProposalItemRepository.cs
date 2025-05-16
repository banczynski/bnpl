using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IProposalItemRepository
    {
        Task InsertAsync(ProposalItem proposalItem);
        Task InsertManyAsync(IEnumerable<ProposalItem> items);
        Task<IEnumerable<ProposalItem>> GetByProposalIdAsync(Guid proposalId);
        Task MarkAsReturnedAsync(Guid proposalId, Guid productId, string reason);
    }
}
