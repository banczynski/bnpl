using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GenerateSignatureLinkUseCase(
        IProposalRepository proposalRepository,
        ICustomerRepository customerRepository,
        IKycRepository customerKycDataRepository,
        IProposalSignatureRepository proposalSignatureRepository,
        ISignatureService signatureService,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<Uri>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status is not (ProposalStatus.AwaitingSignature or ProposalStatus.AwaitingKyc))
                throw new InvalidOperationException("Proposal is not eligible for signature.");

            var customer = await customerRepository.GetByIdAsync(proposal.CustomerId)
                ?? throw new InvalidOperationException("Customer not found.");

            var kyc = await customerKycDataRepository.GetByCustomerIdAsync(proposal.CustomerId)
                ?? throw new InvalidOperationException("KYC data not found.");

            if (!kyc.OcrValidated || !kyc.FaceMatchValidated)
                throw new InvalidOperationException("Customer has not completed identity validation.");

            // TODO
            var link = await signatureService.GenerateSignatureLinkAsync(new SignatureRequest(
                ProposalId: proposal.Id,
                CustomerName: customer.Name,
                CustomerTaxId: proposal.CustomerTaxId,
                ApprovedAmount: proposal.ApprovedAmount,
                Installments: proposal.Installments
            ));

            proposal.MarkAsAwaitingSignature(DateTime.UtcNow, userContext.UserId);

            await proposalRepository.UpdateAsync(proposal);

            await proposalSignatureRepository.InsertAsync(new ProposalSignature
            {
                ProposalId = proposal.Id,
                ExternalSignatureId = link.ToString(),
                Status = SignatureStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userContext.UserId,
                UpdatedBy = userContext.UserId
            });

            return new ServiceResult<Uri>(link, ["Signature link generated."]);
        }
    }
}
