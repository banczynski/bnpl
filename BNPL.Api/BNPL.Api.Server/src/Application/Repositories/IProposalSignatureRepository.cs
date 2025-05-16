using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IProposalSignatureRepository
    {
        Task InsertAsync(ProposalSignature signature);
        Task UpdateAsync(ProposalSignature signature);
        Task<ProposalSignature?> GetByProposalIdAsync(Guid proposalId);
        Task<ProposalSignature?> GetByExternalIdAsync(string externalSignatureId);
    }
}
