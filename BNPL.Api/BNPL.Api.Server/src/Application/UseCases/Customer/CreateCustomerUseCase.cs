using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class CreateCustomerUseCase(
        ICustomerRepository repository,
        IKycRepository kycRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CreateCustomerResponse>> ExecuteAsync(CreateCustomerRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var entity = request.ToEntity(id, now, userContext.UserId);
            await repository.InsertAsync(entity);

            if (request.SkipKyc)
            {
                var existingKyc = await kycRepository.GetByCustomerIdAsync(id);
                if (existingKyc is null)
                {
                    var kyc = new Domain.Entities.Kyc
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = id,
                        Status = KycStatus.Validated,
                        OcrValidated = true,
                        FaceMatchValidated = true,
                        CreatedAt = now,
                        UpdatedAt = now,
                        CreatedBy = userContext.UserId,
                        UpdatedBy = userContext.UserId,
                        IsActive = true
                    };

                    await kycRepository.InsertAsync(kyc);
                }
            }

            return new ServiceResult<CreateCustomerResponse>(
                new CreateCustomerResponse(entity.Id),
                ["Customer created successfully."]
            );
        }
    }
}
