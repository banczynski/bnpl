using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class UpdateKycUseCase(
        IKycRepository kycRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<KycDto, string>> ExecuteAsync(Guid customerId, UpdateKycRequest request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId);

            if (entity is null)
                return Result<KycDto, string>.Fail("KYC data not found.");

            entity.UpdateEntity(request, DateTime.UtcNow, userContext.GetRequiredUserId());
            await kycRepository.UpdateAsync(entity);

            return Result<KycDto, string>.Ok(entity.ToDto());
        }
    }
}
