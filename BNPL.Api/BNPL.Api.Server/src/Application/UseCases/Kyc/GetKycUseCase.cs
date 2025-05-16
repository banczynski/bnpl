using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class GetKycUseCase(IKycRepository repository)
    {
        public async Task<ServiceResult<KycDto>> ExecuteAsync(Guid customerId)
        {
            var entity = await repository.GetByCustomerIdAsync(customerId)
                ?? throw new InvalidOperationException("KYC data not found.");

            return new ServiceResult<KycDto>(entity.ToDto());
        }
    }
}
