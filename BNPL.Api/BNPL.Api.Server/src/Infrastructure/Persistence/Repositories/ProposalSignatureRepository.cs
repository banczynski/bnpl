using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalSignatureRepository(IDbConnection connection) : GenericRepository<ProposalSignature>(connection), IProposalSignatureRepository
    {
        private const string GetByProposalIdSql = "SELECT * FROM proposal_signature WHERE proposal_id = @ProposalId LIMIT 1";
        private const string GetByExternalIdSql = "SELECT * FROM proposal_signature WHERE external_signature_id = @ExternalSignatureId LIMIT 1";

        public async Task<ProposalSignature?> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<ProposalSignature>(GetByProposalIdSql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<ProposalSignature?> GetByExternalIdAsync(string externalSignatureId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<ProposalSignature>(GetByExternalIdSql, new { ExternalSignatureId = externalSignatureId }, transaction);
        }
    }
}