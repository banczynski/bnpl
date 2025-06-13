using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class KycRepository(IDbConnection connection) : GenericRepository<Kyc>(connection), IKycRepository
    {
        private const string GetByCustomerIdSql = "SELECT * FROM kyc WHERE customer_id = @CustomerId LIMIT 1";

        public async Task<Kyc?> GetByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<Kyc>(GetByCustomerIdSql, new { CustomerId = customerId }, transaction);
        }
    }
}