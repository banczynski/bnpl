using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IKycRepository : IGenericRepository<Kyc>
    {
        Task<Kyc?> GetByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
    }
}