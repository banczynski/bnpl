using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class UpdateKycUseCase(
        IKycRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid customerId, UpdateKycRequest request)
        {
            var entity = await repository.GetByCustomerIdAsync(customerId)
                ?? throw new InvalidOperationException("KYC data not found.");

            entity.UpdateEntity(request, DateTime.UtcNow, userContext.UserId);
            await repository.UpdateAsync(entity);

            return new ServiceResult<string>("KYC data updated.");
        }
    }
}
