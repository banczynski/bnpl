using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class UpsertCustomerCreditLimitUseCase(
        ICustomerCreditLimitRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(CreditLimitUpsertRequest request)
        {
            var existing = await repository.GetByTaxIdAsync(request.CustomerTaxId);
            var now = DateTime.UtcNow;

            if (existing is null)
            {
                var newEntity = new CustomerCreditLimit
                {
                    Id = Guid.NewGuid(),
                    PartnerId = request.PartnerId,
                    AffiliateId = request.AffiliateId,
                    CustomerTaxId = request.CustomerTaxId,
                    ApprovedLimit = request.ApprovedLimit,
                    UsedLimit = 0,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = userContext.UserId,
                    UpdatedBy = userContext.UserId,
                    IsActive = true
                };

                await repository.InsertAsync(newEntity);
            }
            else
            {
                existing.ApprovedLimit = request.ApprovedLimit;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userContext.UserId;
                await repository.UpdateAsync(existing);
            }

            return new ServiceResult<string>("Customer credit limit upserted.");
        }
    }
}
