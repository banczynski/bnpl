using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IAffiliateRepository
    {
        Task InsertAsync(Affiliate affiliate);
        Task UpdateAsync(Affiliate affiliate);
        Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt);
        Task<Affiliate?> GetByIdAsync(Guid id);
        Task<IEnumerable<Affiliate>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true);
    }
}
