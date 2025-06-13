using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class CreateKycUseCase(
        IKycRepository kycRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<KycDto, string>> ExecuteAsync(Guid customerId, CreateKycRequest request)
        {
            var existing = await kycRepository.GetByCustomerIdAsync(customerId);
            if (existing is not null)
                return Result<KycDto, string>.Fail("KYC already exists for this customer.");

            var entity = request.ToEntity(customerId, userContext.GetRequiredUserId());
            await kycRepository.InsertAsync(entity);

            return Result<KycDto, string>.Ok(entity.ToDto());
        }
    }
}
