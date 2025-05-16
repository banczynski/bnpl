using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class ProposalSignatureRepository(IDbConnection connection) : IProposalSignatureRepository
    {
        public async Task InsertAsync(ProposalSignature signature)
            => await connection.InsertAsync(signature);

        public async Task UpdateAsync(ProposalSignature signature)
            => await connection.UpdateAsync(signature);

        public async Task<ProposalSignature?> GetByProposalIdAsync(Guid proposalId)
        {
            const string sql = "SELECT * FROM proposal_signature WHERE proposal_id = @ProposalId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<ProposalSignature>(sql, new { ProposalId = proposalId });
        }

        public async Task<ProposalSignature?> GetByExternalIdAsync(string externalSignatureId)
        {
            const string sql = "SELECT * FROM proposal_signature WHERE external_signature_id = @ExternalSignatureId LIMIT 1";
            return await connection.QueryFirstOrDefaultAsync<ProposalSignature>(sql, new { ExternalSignatureId = externalSignatureId });
        }
    }
}
