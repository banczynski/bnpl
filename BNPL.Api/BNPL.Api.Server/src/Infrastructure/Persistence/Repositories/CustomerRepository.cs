using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private const string GetIdByTaxIdSql = "SELECT code FROM customer WHERE tax_id = @TaxId LIMIT 1";
        private const string GetByTaxIdSql = "SELECT * FROM customer WHERE tax_id = @TaxId";
        private const string GetActivesByAffiliateIdSql = "SELECT * FROM customer WHERE affiliate_id = @AffiliateId AND is_active = TRUE";
        private const string GetAllByAffiliateIdSql = "SELECT * FROM customer WHERE affiliate_id = @AffiliateId";
        private const string GetActivesByPartnerIdSql = "SELECT * FROM customer WHERE partner_id = @PartnerId AND is_active = TRUE";
        private const string GetAllByPartnerIdSql = "SELECT * FROM customer WHERE partner_id = @PartnerId";

        public CustomerRepository(IDbConnection connection) : base(connection) { }

        public async Task<Guid?> GetIdByTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            return await Connection.QuerySingleOrDefaultAsync<Guid?>(GetIdByTaxIdSql, new { TaxId = taxId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetByTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Customer>(GetByTaxIdSql, new { TaxId = taxId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetActivesByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Customer>(GetActivesByAffiliateIdSql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetAllByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Customer>(GetAllByAffiliateIdSql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Customer>(GetActivesByPartnerIdSql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<Customer>> GetAllByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Customer>(GetAllByPartnerIdSql, new { PartnerId = partnerId }, transaction);
        }
    }
}