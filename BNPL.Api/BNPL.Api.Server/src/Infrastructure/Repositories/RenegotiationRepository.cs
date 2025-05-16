using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class RenegotiationRepository(IDbConnection connection) : IRenegotiationRepository
    {
        public async Task InsertAsync(Renegotiation renegotiation)
            => await connection.InsertAsync(renegotiation);

        public async Task UpdateAsync(Renegotiation renegotiation)
            => await connection.UpdateAsync(renegotiation);

        public async Task<Renegotiation?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Renegotiation>(id);

        public async Task<IEnumerable<Renegotiation>> GetByCustomerIdAsync(Guid customerId)
        {
            const string sql = """
            SELECT * FROM renegotiation
            WHERE customer_id = @CustomerId
            AND is_active = TRUE
            """;

            return await connection.QueryAsync<Renegotiation>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Renegotiation>> GetByPartnerIdAsync(Guid partnerId)
        {
            const string sql = """
            SELECT * FROM renegotiation
            WHERE partner_id = @PartnerId AND is_active = TRUE
            """;

            return await connection.QueryAsync<Renegotiation>(sql, new { PartnerId = partnerId });
        }

        public async Task<IEnumerable<Renegotiation>> GetByAffiliateIdAsync(Guid affiliateId)
        {
            const string sql = """
            SELECT * FROM renegotiation
            WHERE affiliate_id = @AffiliateId AND is_active = TRUE
            """;

            return await connection.QueryAsync<Renegotiation>(sql, new { AffiliateId = affiliateId });
        }
    }
}
