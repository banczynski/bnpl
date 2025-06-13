using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using System.Data;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class AdjustCustomerCreditLimitUseCase(
        ICustomerCreditLimitRepository customerCreditLimitRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, Error>> ExecuteAsync(string taxId, Guid affiliateId, decimal value, bool increase, IDbTransaction? transaction = null)
        {
            var entity = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(taxId, affiliateId, transaction);
            if (entity is null)
                return Result<bool, Error>.Fail(DomainErrors.CreditLimit.NotFound);

            var now = DateTime.UtcNow;
            if (increase)
                entity.DecreaseUsedLimit(value, now, userContext.GetRequiredUserId());
            else
                entity.IncreaseUsedLimit(value, now, userContext.GetRequiredUserId());

            await customerCreditLimitRepository.UpdateAsync(entity, transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}