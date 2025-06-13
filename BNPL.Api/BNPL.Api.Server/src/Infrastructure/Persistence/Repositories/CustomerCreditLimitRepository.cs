using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerCreditLimitRepository(IDbConnection connection) : ICustomerCreditLimitRepository
    {
        public async Task InsertAsync(CustomerCreditLimit limit, IDbTransaction? transaction = null)
            => await connection.InsertAsync(limit, transaction);

        public async Task UpdateAsync(CustomerCreditLimit limit, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(limit, transaction);

        public async Task<CustomerCreditLimit?> GetByTaxIdAndAffiliateIdAsync(string taxId, Guid affiliateId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM customer_credit_limit WHERE customer_tax_id = @TaxId AND affiliate_id = @AffiliateId AND is_active = TRUE";
            return await connection.QueryFirstOrDefaultAsync<CustomerCreditLimit>(sql, new { TaxId = taxId, AffiliateId = affiliateId }, transaction);
        }
    }
}
