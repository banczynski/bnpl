using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using System.Data;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class UpsertCustomerCreditLimitUseCase(
        ICustomerCreditLimitRepository customerCreditLimitRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, Error>> ExecuteAsync(CreditLimitUpsertRequest request, IDbTransaction? transaction = null)
        {
            var existing = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(request.CustomerTaxId, request.AffiliateId, transaction);
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            if (existing is null)
            {
                var newEntity = new CustomerCreditLimit
                {
                    Code = Guid.NewGuid(),
                    PartnerId = request.PartnerId,
                    AffiliateId = request.AffiliateId,
                    CustomerTaxId = request.CustomerTaxId,
                    ApprovedLimit = request.ApprovedLimit,
                    UsedLimit = 0,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    IsActive = true
                };

                await customerCreditLimitRepository.InsertAsync(newEntity, transaction);
            }
            else
            {
                existing.ApprovedLimit = request.ApprovedLimit;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                await customerCreditLimitRepository.UpdateAsync(existing, transaction);
            }

            return Result<bool, Error>.Ok(true);
        }
    }
}