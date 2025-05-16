using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class KycRepository(IDbConnection connection) : IKycRepository
    {
        public async Task InsertAsync(Kyc data)
            => await connection.InsertAsync(data);

        public async Task UpdateAsync(Kyc data)
            => await connection.UpdateAsync(data);

        public async Task<Kyc?> GetByCustomerIdAsync(Guid customerId)
        {
            const string sql = "SELECT * FROM customer_kyc_data WHERE customer_id = @CustomerId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<Kyc>(sql, new { CustomerId = customerId });
        }
    }
}
