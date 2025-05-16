using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface ICustomerCreditLimitRepository
    {
        Task<CustomerCreditLimit?> GetByTaxIdAsync(string taxId);
        Task InsertAsync(CustomerCreditLimit limit);
        Task UpdateAsync(CustomerCreditLimit limit);
    }
}
