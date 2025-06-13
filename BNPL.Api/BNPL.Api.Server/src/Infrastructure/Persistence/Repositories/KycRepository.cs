using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class KycRepository(IDbConnection connection) : IKycRepository
    {
        public async Task InsertAsync(Kyc data, IDbTransaction? transaction = null)
            => await connection.InsertAsync(data, transaction);

        public async Task UpdateAsync(Kyc data, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(data, transaction);

        public async Task<Kyc?> GetByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM kyc WHERE customer_id = @CustomerId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<Kyc>(sql, new { CustomerId = customerId }, transaction);
        }
    }
}
