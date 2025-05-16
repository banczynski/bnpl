using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class CreateKycUseCase(
        IKycRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(CreateKycRequest request)
        {
            var existing = await repository.GetByCustomerIdAsync(request.CustomerId);
            if (existing is not null)
                throw new InvalidOperationException("KYC already exists for this customer.");

            var now = DateTime.UtcNow;

            var entity = request.ToEntity(now, userContext.UserId);
            await repository.InsertAsync(entity);

            return new ServiceResult<string>("KYC data saved.");
        }
    }
}
