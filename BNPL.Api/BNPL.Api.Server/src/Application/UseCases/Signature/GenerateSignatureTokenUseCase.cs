using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Signature
{
    public sealed class GenerateSignatureTokenUseCase(
        IProposalRepository proposalRepository,
        ICustomerRepository customerRepository,
        IKycRepository kycRepository,
        IProposalSignatureRepository proposalSignatureRepository,
        ISignatureService signatureService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<SignatureTokenResponse, string>> ExecuteAsync(Guid proposalId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                if (proposal is null)
                    return Result<SignatureTokenResponse, string>.Fail("Proposal not found.");

                if (proposal.Status is not (ProposalStatus.Approved or ProposalStatus.AwaitingSignature))
                    return Result<SignatureTokenResponse, string>.Fail("Proposal is not eligible for signature.");

                var customer = await customerRepository.GetByIdAsync(proposal.CustomerId, scope.Transaction);
                if (customer is null)
                    return Result<SignatureTokenResponse, string>.Fail("Customer not found.");

                var kyc = await kycRepository.GetByCustomerIdAsync(proposal.CustomerId, scope.Transaction);
                if (kyc is null || !kyc.OcrValidated || !kyc.FaceMatchValidated)
                    return Result<SignatureTokenResponse, string>.Fail("Customer has not completed identity validation.");

                proposal.MarkAsAwaitingSignature(DateTime.UtcNow, userContext.GetRequiredUserId());
                await proposalRepository.UpdateAsync(proposal, scope.Transaction);

                var tokenResult = await signatureService.GenerateSignatureTokenAsync(proposalId, customer.Phone);

                await proposalSignatureRepository.InsertAsync(new ProposalSignature
                {
                    Code = Guid.NewGuid(),
                    ProposalId = proposal.Code,
                    ExpiresAt = tokenResult.ExpiresAt,
                    Destination = tokenResult.Destination,
                    Status = SignatureStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userContext.GetRequiredUserId(),
                    UpdatedBy = userContext.GetRequiredUserId()
                }, scope.Transaction);

                scope.Commit();

                return Result<SignatureTokenResponse, string>.Ok(tokenResult);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
