using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerBillingPreferencesRepository(IDbConnection connection) : ICustomerBillingPreferencesRepository
    {
        public async Task InsertAsync(CustomerBillingPreferences entity, IDbTransaction? transaction = null)
            => await connection.InsertAsync(entity, transaction);

        public async Task UpdateAsync(CustomerBillingPreferences entity, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(entity, transaction);

        public async Task<CustomerBillingPreferences?> GetByCustomerIdAndAffiliateIdAsync(Guid customerId, Guid affiliateId, IDbTransaction? transaction = null)
        {
            const string sql = """
                SELECT * 
                FROM customer_billing_preferences
                WHERE customer_id = @CustomerId
                  AND affiliate_id = @AffiliateId
                LIMIT 1;
                """;

            return await connection.QueryFirstOrDefaultAsync<CustomerBillingPreferences>(
                sql,
                new { CustomerId = customerId, AffiliateId = affiliateId },
                transaction
            );
        }
    }
}
