using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IPartnerRepository
    {
        Task InsertAsync(Partner partner, IDbTransaction? transaction = null);
        Task UpdateAsync(Partner partner, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null);
        Task<Partner?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<IEnumerable<Partner>> GetAllAsync(bool onlyActive = true, IDbTransaction? transaction = null);
    }
}
