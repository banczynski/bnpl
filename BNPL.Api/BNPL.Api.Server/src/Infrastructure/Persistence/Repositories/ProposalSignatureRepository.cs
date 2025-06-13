using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class ProposalSignatureRepository(IDbConnection connection) : IProposalSignatureRepository
    {
        public async Task InsertAsync(ProposalSignature signature, IDbTransaction? transaction = null)
            => await connection.InsertAsync(signature, transaction);

        public async Task UpdateAsync(ProposalSignature signature, IDbTransaction? transaction = null)
            => await connection.UpdateAsync(signature, transaction);

        public async Task<ProposalSignature?> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM proposal_signature WHERE proposal_id = @ProposalId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<ProposalSignature>(sql, new { ProposalId = proposalId }, transaction);
        }

        public async Task<ProposalSignature?> GetByExternalIdAsync(string externalSignatureId, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM proposal_signature WHERE external_signature_id = @ExternalSignatureId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<ProposalSignature>(sql, new { ExternalSignatureId = externalSignatureId }, transaction);
        }
    }
}
