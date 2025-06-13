using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ICustomerCreditLimitRepository
    {
        Task InsertAsync(CustomerCreditLimit limit, IDbTransaction? transaction = null);
        Task UpdateAsync(CustomerCreditLimit limit, IDbTransaction? transaction = null);
        Task<CustomerCreditLimit?> GetByTaxIdAndAffiliateIdAsync(string taxId, Guid affiliateId, IDbTransaction? transaction = null);
    }
}
