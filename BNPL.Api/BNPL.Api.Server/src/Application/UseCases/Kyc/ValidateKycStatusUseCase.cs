using BNPL.Api.Server.src.Application.Context;
using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class ValidateKycStatusUseCase(
        IKycRepository repository,
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid customerId)
        {
            var entity = await repository.GetByCustomerIdAsync(customerId)
                ?? throw new InvalidOperationException("KYC data not found.");

            if (entity.OcrValidated && entity.FaceMatchValidated)
            {
                entity.MarkAsValidated(DateTime.UtcNow, userContext.UserId);

                await repository.UpdateAsync(entity);

                var proposal = await proposalRepository.GetByCustomerIdAsync(customerId);

                if (proposal != null)
                {
                    proposal.MarkAsAwaitingSignature(DateTime.UtcNow, userContext.UserId);
                    await proposalRepository.UpdateAsync(proposal);
                }

                return new ServiceResult<string>("KYC status set to Validated.");
            }

            return new ServiceResult<string>(
                "KYC not fully validated.",
                [$"OCR: {entity.OcrValidated}", $"FaceMatch: {entity.FaceMatchValidated}"]
            );
        }
    }
}
