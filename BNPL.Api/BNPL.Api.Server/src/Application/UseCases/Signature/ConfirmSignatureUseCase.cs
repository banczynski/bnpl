using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.Proposal;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Signature
{
    public sealed class ConfirmSignatureUseCase(
        IProposalRepository proposalRepository,
        IProposalSignatureRepository proposalSignatureRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId, string token)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                if (proposal is null)
                    return Result<bool, string>.Fail("Proposal not found.");

                if (proposal.Status != ProposalStatus.AwaitingSignature)
                    return Result<bool, string>.Fail("Proposal is not awaiting signature.");

                var signature = await proposalSignatureRepository.GetByProposalIdAsync(proposal.Code, scope.Transaction);
                if (signature is null || signature.ExternalSignatureId != token || signature.Status != SignatureStatus.Pending)
                    return Result<bool, string>.Fail("Invalid or expired token.");

                proposal.MarkAsSigned(DateTime.UtcNow, userContext.GetRequiredUserId());
                await proposalRepository.UpdateAsync(proposal, scope.Transaction);

                signature.MarkAsSigned(DateTime.UtcNow, userContext.GetRequiredUserId());
                await proposalSignatureRepository.UpdateAsync(signature, scope.Transaction);

                scope.Commit();

                return Result<bool, string>.Ok(true);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
