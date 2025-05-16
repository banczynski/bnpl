using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IKycRepository
    {
        Task InsertAsync(Kyc data);
        Task UpdateAsync(Kyc data);
        Task<Kyc?> GetByCustomerIdAsync(Guid customerId);
    }
}
