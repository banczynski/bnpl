using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

public interface IAffiliateRepository : IGenericRepository<Affiliate>
{
    Task<Guid?> GetPartnerIdByAffiliateIdAsync(Guid code, IDbTransaction? transaction = null);
    Task<IEnumerable<Affiliate>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
    Task<IEnumerable<Affiliate>> GetAllByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
}