using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ICustomerBillingPreferencesRepository
    {
        Task InsertAsync(CustomerBillingPreferences entity, IDbTransaction? transaction = null);
        Task UpdateAsync(CustomerBillingPreferences entity, IDbTransaction? transaction = null);
        Task<CustomerBillingPreferences?> GetByCustomerIdAndAffiliateIdAsync(Guid customerId, Guid affiliateId, IDbTransaction? transaction = null);
    }
}
