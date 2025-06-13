using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IProposalRepository
    {
        Task InsertAsync(Proposal proposal, IDbTransaction? transaction = null);
        Task UpdateAsync(Proposal proposal, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null);
        Task UpdateManyAsync(IEnumerable<Proposal> proposals, IDbTransaction? transaction = null);
        Task<Proposal?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<Proposal?> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetListByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync(IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<IEnumerable<Proposal>> GetPendingBeforeDateAsync(DateTime cutoff, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<bool> ExistsActiveByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
    }
}
