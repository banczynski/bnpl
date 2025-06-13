using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed record UpdateKycRequestUseCase(Guid CustomerId, UpdateKycRequest Dto);

    public sealed class UpdateKycUseCase(
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateKycRequestUseCase, Result<KycDto, Error>>
    {
        public async Task<Result<KycDto, Error>> ExecuteAsync(UpdateKycRequestUseCase request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (entity is null)
                return Result<KycDto, Error>.Fail(DomainErrors.Kyc.NotFound);

            entity.UpdateEntity(request.Dto, DateTime.UtcNow, userContext.GetRequiredUserId());
            await kycRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<KycDto, Error>.Ok(entity.ToDto());
        }
    }
}