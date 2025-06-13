using Core.Context.Interfaces;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class ValidateFaceMatchUseCase(
        IKycRepository kycRepository,
        IFaceMatchService faceMatchService,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid customerId)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId);

            if (entity is null)
                return Result<bool, string>.Fail("KYC data not found.");

            if (string.IsNullOrWhiteSpace(entity.DocumentImageUrl) || string.IsNullOrWhiteSpace(entity.SelfieImageUrl))
                return Result<bool, string>.Fail("Missing images for face validation.");

            var valid = await faceMatchService.ValidateAsync(
                new Uri(entity.DocumentImageUrl),
                new Uri(entity.SelfieImageUrl)
            );

            if (!valid)
                return Result<bool, string>.Fail("Face match validation failed.");

            entity.FaceMatchValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.GetRequiredUserId();

            await kycRepository.UpdateAsync(entity);

            return Result<bool, string>.Ok(true);
        }
    }
}
