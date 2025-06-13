using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class GetKycUseCase(IKycRepository kycRepository)
    {
        public async Task<Result<KycDto, string>> ExecuteAsync(Guid customerId)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId);

            if (entity is null)
                return Result<KycDto, string>.Fail("KYC data not found.");

            return Result<KycDto, string>.Ok(entity.ToDto());
        }
    }
}
