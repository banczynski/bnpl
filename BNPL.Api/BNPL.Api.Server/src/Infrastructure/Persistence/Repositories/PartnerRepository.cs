using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class PartnerRepository(IDbConnection connection) : GenericRepository<Partner>(connection), IPartnerRepository
    {
        private const string GetAllSql = "SELECT * FROM partner";
        private const string GetActivesSql = "SELECT * FROM partner WHERE is_active = TRUE";

        public async Task<IEnumerable<Partner>> GetAllAsync(IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Partner>(GetAllSql, transaction: transaction);
        }

        public async Task<IEnumerable<Partner>> GetActivesAsync(IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Partner>(GetActivesSql, transaction: transaction);
        }
    }
}