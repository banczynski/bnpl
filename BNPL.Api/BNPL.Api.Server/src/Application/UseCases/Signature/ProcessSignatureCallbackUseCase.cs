using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Signature
{
    public sealed class ProcessSignatureCallbackUseCase(
        IProposalSignatureRepository signatureRepository,
        IProposalRepository proposalRepository,
        ISignatureService signatureService,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(string externalSignatureId)
        {
            var signature = await signatureRepository.GetByExternalIdAsync(externalSignatureId)
                ?? throw new InvalidOperationException("Signature not found.");

            var proposal = await proposalRepository.GetByIdAsync(signature.ProposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status != ProposalStatus.AwaitingSignature)
                throw new InvalidOperationException("Cannot mark as signed if not awaiting signature.");

            // TODO
            var status = await signatureService.CheckStatusAsync(externalSignatureId);
            if (signature.Status == status)
                return new ServiceResult<string>("Status already up to date.");

            signature.Status = status;
            signature.UpdatedAt = DateTime.UtcNow;
            signature.UpdatedBy = userContext.UserId;

            await signatureRepository.UpdateAsync(signature);

            if (status == SignatureStatus.Signed)
            {
                proposal.MarkAsSigned(DateTime.UtcNow, userContext.UserId);

                await proposalRepository.UpdateAsync(proposal);
            }

            return new ServiceResult<string>("Signature status updated.", [$"New status: {status}"]);
        }
    }
}
