using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IKycRepository
    {
        Task InsertAsync(Kyc data, IDbTransaction? transaction = null);
        Task UpdateAsync(Kyc data, IDbTransaction? transaction = null);
        Task<Kyc?> GetByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
    }
}
