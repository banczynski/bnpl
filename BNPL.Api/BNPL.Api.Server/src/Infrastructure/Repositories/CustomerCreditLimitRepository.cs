using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class CustomerCreditLimitRepository(IDbConnection connection) : ICustomerCreditLimitRepository
    {
        public async Task<CustomerCreditLimit?> GetByTaxIdAsync(string taxId)
        {
            const string sql = "SELECT * FROM customer_credit_limit WHERE customer_tax_id = @TaxId AND is_active = TRUE";
            return await connection.QueryFirstOrDefaultAsync<CustomerCreditLimit>(sql, new { TaxId = taxId });
        }

        public async Task InsertAsync(CustomerCreditLimit limit)
            => await connection.InsertAsync(limit);

        public async Task UpdateAsync(CustomerCreditLimit limit)
            => await connection.UpdateAsync(limit);
    }
}
