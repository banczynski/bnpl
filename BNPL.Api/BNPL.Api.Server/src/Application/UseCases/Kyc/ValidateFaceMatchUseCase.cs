using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class ValidateFaceMatchUseCase(
        IKycRepository repository,
        IFaceMatchService faceMatchService,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid customerId)
        {
            var entity = await repository.GetByCustomerIdAsync(customerId)
                ?? throw new InvalidOperationException("KYC data not found.");

            if (string.IsNullOrWhiteSpace(entity.DocumentImageUrl) || string.IsNullOrWhiteSpace(entity.SelfieImageUrl))
                throw new InvalidOperationException("Missing images for face validation.");

            // TODO
            var valid = await faceMatchService.ValidateAsync(
                new Uri(entity.DocumentImageUrl),
                new Uri(entity.SelfieImageUrl)
            );

            if (!valid)
                throw new InvalidOperationException("Face match validation failed.");

            entity.FaceMatchValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.UserId;

            await repository.UpdateAsync(entity);

            return new ServiceResult<string>("Face match validation successful.");
        }
    }
}
