using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<IEnumerable<Partner>> GetAllAsync(IDbTransaction? transaction = null);
        Task<IEnumerable<Partner>> GetActivesAsync(IDbTransaction? transaction = null);
    }
}