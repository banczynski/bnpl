using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed record CreateKycRequestUseCase(Guid CustomerId, CreateKycRequest Dto);

    public sealed class CreateKycUseCase(
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateKycRequestUseCase, Result<KycDto, Error>>
    {
        public async Task<Result<KycDto, Error>> ExecuteAsync(CreateKycRequestUseCase request)
        {
            var existing = await kycRepository.GetByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (existing is not null)
                return Result<KycDto, Error>.Fail(DomainErrors.Kyc.AlreadyExists);

            var entity = request.Dto.ToEntity(request.CustomerId, userContext.GetRequiredUserId());
            await kycRepository.InsertAsync(entity, unitOfWork.Transaction);

            return Result<KycDto, Error>.Ok(entity.ToDto());
        }
    }
}