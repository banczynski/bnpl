using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IProposalSignatureRepository
    {
        Task InsertAsync(ProposalSignature signature, IDbTransaction? transaction = null);
        Task UpdateAsync(ProposalSignature signature, IDbTransaction? transaction = null);
        Task<ProposalSignature?> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null);
        Task<ProposalSignature?> GetByExternalIdAsync(string externalSignatureId, IDbTransaction? transaction = null);
    }
}
