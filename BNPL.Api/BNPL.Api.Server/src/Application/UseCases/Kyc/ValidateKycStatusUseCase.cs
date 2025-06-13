using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed record ValidateKycStatusRequestUseCase(Guid CustomerId);

    public sealed class ValidateKycStatusUseCase(
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<ValidateKycStatusRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(ValidateKycStatusRequestUseCase request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (entity is null)
                return Result<bool, Error>.Fail(DomainErrors.Kyc.NotFound);

            if (entity.OcrValidated && entity.FaceMatchValidated)
            {
                entity.MarkAsValidated(DateTime.UtcNow, userContext.GetRequiredUserId());
                await kycRepository.UpdateAsync(entity, unitOfWork.Transaction);
                return Result<bool, Error>.Ok(true);
            }

            return Result<bool, Error>.Fail(DomainErrors.Kyc.NotFullyValidated);
        }
    }
}