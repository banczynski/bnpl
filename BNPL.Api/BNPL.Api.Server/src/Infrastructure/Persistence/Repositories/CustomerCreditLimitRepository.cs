using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerCreditLimitRepository(IDbConnection connection) : GenericRepository<CustomerCreditLimit>(connection), ICustomerCreditLimitRepository
    {
        private const string GetByTaxIdAndAffiliateSql = "SELECT * FROM customer_credit_limit WHERE customer_tax_id = @TaxId AND affiliate_id = @AffiliateId AND is_active = TRUE";

        public async Task<CustomerCreditLimit?> GetByTaxIdAndAffiliateIdAsync(string taxId, Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<CustomerCreditLimit>(GetByTaxIdAndAffiliateSql, new { TaxId = taxId, AffiliateId = affiliateId }, transaction);
        }
    }
}