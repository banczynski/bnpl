using BNPL.Api.Server.src.Application.Abstractions.External;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed record ValidateFaceMatchRequestUseCase(Guid CustomerId);

    public sealed class ValidateFaceMatchUseCase(
        IKycRepository kycRepository,
        IFaceMatchService faceMatchService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<ValidateFaceMatchRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(ValidateFaceMatchRequestUseCase request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (entity is null)
                return Result<bool, Error>.Fail(DomainErrors.Kyc.NotFound);

            if (string.IsNullOrWhiteSpace(entity.DocumentImageUrl) || string.IsNullOrWhiteSpace(entity.SelfieImageUrl))
                return Result<bool, Error>.Fail(DomainErrors.Kyc.MissingImages);

            var valid = await faceMatchService.ValidateAsync(
                new Uri(entity.DocumentImageUrl),
                new Uri(entity.SelfieImageUrl)
            );
            if (!valid)
                return Result<bool, Error>.Fail(DomainErrors.Kyc.FaceMatchFailed);

            entity.FaceMatchValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.GetRequiredUserId();

            await kycRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}