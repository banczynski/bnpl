using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class GetKycUseCase(IKycRepository kycRepository)
    {
        public async Task<Result<KycDto, Error>> ExecuteAsync(Guid customerId)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId);

            if (entity is null)
                return Result<KycDto, Error>.Fail(DomainErrors.Kyc.NotFound);

            return Result<KycDto, Error>.Ok(entity.ToDto());
        }
    }
}