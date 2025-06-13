using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class AffiliateRepository(IDbConnection connection) : GenericRepository<Affiliate>(connection), IAffiliateRepository
    {
        private const string GetPartnerIdSql = "SELECT partner_id FROM affiliate WHERE code = @Code LIMIT 1";
        private const string GetActivesByPartnerIdSql = "SELECT * FROM affiliate WHERE partner_id = @PartnerId AND is_active = TRUE";
        private const string GetAllByPartnerIdSql = "SELECT * FROM affiliate WHERE partner_id = @PartnerId";

        public async Task<Guid?> GetPartnerIdByAffiliateIdAsync(Guid code, IDbTransaction? transaction = null)
        {
            return await Connection.QuerySingleOrDefaultAsync<Guid?>(GetPartnerIdSql, new { Code = code }, transaction);
        }

        public async Task<IEnumerable<Affiliate>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Affiliate>(GetActivesByPartnerIdSql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<Affiliate>> GetAllByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Affiliate>(GetAllByPartnerIdSql, new { PartnerId = partnerId }, transaction);
        }
    }
}