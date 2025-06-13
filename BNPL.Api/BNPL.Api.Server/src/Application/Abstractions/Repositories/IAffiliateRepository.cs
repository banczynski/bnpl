using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IAffiliateRepository
    {
        Task InsertAsync(Affiliate affiliate, IDbTransaction? transaction = null);
        Task UpdateAsync(Affiliate affiliate, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid id, Guid updatedBy, IDbTransaction? transaction = null);
        Task<Affiliate?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<IEnumerable<Affiliate>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<Guid?> GetPartnerIdByAffiliateIdAsync(Guid id, IDbTransaction? transaction = null);
    }
}
