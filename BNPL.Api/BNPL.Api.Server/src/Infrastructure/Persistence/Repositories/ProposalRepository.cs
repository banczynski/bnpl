using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalRepository(IDbConnection connection) : GenericRepository<Proposal>(connection), IProposalRepository
    {
        private const string GetByIdsSql = "SELECT * FROM proposal WHERE code = ANY(@Codes)";
        private const string GetSignedWithoutInvoiceSql = "SELECT * FROM proposal WHERE status = @Status AND is_active = TRUE AND id NOT IN (SELECT UNNEST(proposal_ids) FROM invoice WHERE is_active = TRUE)";
        private const string GetActivesByCustomerSql = "SELECT * FROM proposal WHERE customer_id = @CustomerId AND is_active = TRUE";
        private const string GetActivesByAffiliateSql = "SELECT * FROM proposal WHERE affiliate_id = @AffiliateId AND is_active = TRUE";
        private const string GetActivesByPartnerSql = "SELECT * FROM proposal WHERE partner_id = @PartnerId AND is_active = TRUE";
        private const string GetPendingBeforeDateSql = "SELECT * FROM proposal WHERE status IN (@Created, @AwaitingSignature) AND created_at < @Cutoff AND is_active = TRUE";
        private const string ExistsActiveByPartnerSql = "SELECT 1 FROM proposal WHERE partner_id = @PartnerId AND is_active = TRUE AND status = @ActiveStatus LIMIT 1";
        private const string ExistsActiveByAffiliateSql = "SELECT 1 FROM proposal WHERE affiliate_id = @AffiliateId AND is_active = TRUE AND status = @ActiveStatus LIMIT 1";
        private const string ExistsActiveByCustomerSql = "SELECT 1 FROM proposal WHERE customer_id = @CustomerId AND is_active = TRUE AND status = @ActiveStatus LIMIT 1";

        public async Task UpdateManyAsync(IEnumerable<Proposal> proposals, IDbTransaction? transaction = null)
        {
            foreach (var p in proposals)
            {
                await base.UpdateAsync(p, transaction);
            }
        }

        public async Task<IEnumerable<Proposal>> GetByIdsAsync(IEnumerable<Guid> codes, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetByIdsSql, new { Codes = codes.ToArray() }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetSignedProposalsWithoutInvoiceAsync(IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetSignedWithoutInvoiceSql, new { Status = ProposalStatus.Signed }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetActivesByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetActivesByCustomerSql, new { CustomerId = customerId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetActivesByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetActivesByAffiliateSql, new { AffiliateId = affiliateId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetActivesByPartnerSql, new { PartnerId = partnerId }, transaction);
        }

        public async Task<IEnumerable<Proposal>> GetPendingBeforeDateAsync(DateTime cutoff, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Proposal>(GetPendingBeforeDateSql, new
            {
                Created = ProposalStatus.Created,
                AwaitingSignature = ProposalStatus.AwaitingSignature,
                Cutoff = cutoff
            }, transaction);
        }

        public async Task<bool> ExistsActiveByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<int?>(ExistsActiveByPartnerSql, new { PartnerId = partnerId, ActiveStatus = ProposalStatus.Active }, transaction);
            return result.HasValue;
        }

        public async Task<bool> ExistsActiveByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<int?>(ExistsActiveByAffiliateSql, new { AffiliateId = affiliateId, ActiveStatus = ProposalStatus.Active }, transaction);
            return result.HasValue;
        }

        public async Task<bool> ExistsActiveByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<int?>(ExistsActiveByCustomerSql, new { CustomerId = customerId, ActiveStatus = ProposalStatus.Active }, transaction);
            return result.HasValue;
        }
    }
}