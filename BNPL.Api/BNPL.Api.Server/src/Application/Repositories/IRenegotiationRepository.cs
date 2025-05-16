using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IRenegotiationRepository
    {
        Task InsertAsync(Renegotiation renegotiation);
        Task UpdateAsync(Renegotiation renegotiation);
        Task<Renegotiation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Renegotiation>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Renegotiation>> GetByPartnerIdAsync(Guid partnerId);
        Task<IEnumerable<Renegotiation>> GetByAffiliateIdAsync(Guid affiliateId);
    }
}
