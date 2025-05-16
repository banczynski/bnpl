using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IProposalRepository
    {
        Task InsertAsync(Proposal proposal);
        Task UpdateAsync(Proposal proposal);
        Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt);
        Task<Proposal?> GetByIdAsync(Guid id);
        Task<Proposal?> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true);
        Task<IEnumerable<Proposal>> GetListByCustomerIdAsync(Guid customerId, bool onlyActive = true);
        Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync();
        Task<IEnumerable<Proposal>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true);
        Task<IEnumerable<Proposal>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true);
    }
}
