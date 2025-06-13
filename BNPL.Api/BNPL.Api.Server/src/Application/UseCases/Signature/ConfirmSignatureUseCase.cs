using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Signature
{
    public sealed record ConfirmSignatureRequestUseCase(Guid ProposalId, string Token);

    public sealed class ConfirmSignatureUseCase(
        IProposalRepository proposalRepository,
        IProposalSignatureRepository proposalSignatureRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<ConfirmSignatureRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(ConfirmSignatureRequestUseCase request)
        {
            var (proposalId, token) = request;

            var proposal = await proposalRepository.GetByIdAsync(proposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != ProposalStatus.AwaitingSignature)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotEligibleForSignature);

            var signature = await proposalSignatureRepository.GetByProposalIdAsync(proposal.Code, unitOfWork.Transaction);
            if (signature is null || signature.ExternalSignatureId != token || signature.Status != SignatureStatus.Pending)
                return Result<bool, Error>.Fail(DomainErrors.Signature.InvalidToken);

            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            proposal.MarkAsSigned(now, userId);
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            signature.MarkAsSigned(now, userId);
            await proposalSignatureRepository.UpdateAsync(signature, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}