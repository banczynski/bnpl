using Core.Models;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid code, IDbTransaction? transaction = null);
        Task<int> InsertAsync(T entity, IDbTransaction? transaction = null);
        Task<bool> UpdateAsync(T entity, IDbTransaction? transaction = null);
        Task<bool> InactivateAsync(Guid code, Guid updatedBy, IDbTransaction? transaction = null);
    }
}