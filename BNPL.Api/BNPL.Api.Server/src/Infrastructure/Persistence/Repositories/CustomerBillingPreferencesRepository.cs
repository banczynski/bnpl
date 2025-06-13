using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerBillingPreferencesRepository(IDbConnection connection) : GenericRepository<CustomerBillingPreferences>(connection), ICustomerBillingPreferencesRepository
    {
        private const string GetByCustomerAndAffiliateSql = "SELECT * FROM customer_billing_preferences WHERE customer_id = @CustomerId AND affiliate_id = @AffiliateId LIMIT 1";

        public async Task<CustomerBillingPreferences?> GetByCustomerIdAndAffiliateIdAsync(Guid customerId, Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<CustomerBillingPreferences>(GetByCustomerAndAffiliateSql, new { CustomerId = customerId, AffiliateId = affiliateId }, transaction);
        }
    }
}