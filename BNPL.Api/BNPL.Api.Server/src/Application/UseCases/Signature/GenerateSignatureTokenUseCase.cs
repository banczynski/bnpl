using BNPL.Api.Server.src.Application.Abstractions.External;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Signature
{
    public sealed record GenerateSignatureTokenRequestUseCase(Guid ProposalId);

    public sealed class GenerateSignatureTokenUseCase(
        IProposalRepository proposalRepository,
        ICustomerRepository customerRepository,
        IKycRepository kycRepository,
        IProposalSignatureRepository proposalSignatureRepository,
        ISignatureService signatureService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<GenerateSignatureTokenRequestUseCase, Result<SignatureTokenResponse, Error>>
    {
        public async Task<Result<SignatureTokenResponse, Error>> ExecuteAsync(GenerateSignatureTokenRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<SignatureTokenResponse, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status is not (ProposalStatus.Approved or ProposalStatus.AwaitingSignature))
                return Result<SignatureTokenResponse, Error>.Fail(DomainErrors.Proposal.NotEligibleForSignature);

            var customer = await customerRepository.GetByIdAsync(proposal.CustomerId, unitOfWork.Transaction);
            if (customer is null)
                return Result<SignatureTokenResponse, Error>.Fail(DomainErrors.Customer.NotFound);

            var kyc = await kycRepository.GetByCustomerIdAsync(proposal.CustomerId, unitOfWork.Transaction);
            if (kyc is null || !kyc.OcrValidated || !kyc.FaceMatchValidated)
                return Result<SignatureTokenResponse, Error>.Fail(DomainErrors.Kyc.NotCompleted);

            var userId = userContext.GetRequiredUserId();
            proposal.MarkAsAwaitingSignature(DateTime.UtcNow, userId);
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            var tokenResult = await signatureService.GenerateSignatureTokenAsync(request.ProposalId, customer.Phone);

            await proposalSignatureRepository.InsertAsync(new ProposalSignature
            {
                Code = Guid.NewGuid(),
                ProposalId = proposal.Code,
                ExternalSignatureId = "123456", // TODO
                ExpiresAt = tokenResult.ExpiresAt,
                Destination = tokenResult.Destination,
                Status = SignatureStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
                IsActive = true
            }, unitOfWork.Transaction);

            return Result<SignatureTokenResponse, Error>.Ok(tokenResult);
        }
    }
}