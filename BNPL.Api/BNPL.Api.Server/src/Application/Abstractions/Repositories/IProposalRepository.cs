using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IProposalRepository : IGenericRepository<Proposal>
    {
        Task UpdateManyAsync(IEnumerable<Proposal> proposals, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> codes, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync(IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetActivesByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetActivesByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetPendingBeforeDateAsync(DateTime cutoff, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
    }
}