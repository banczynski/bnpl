using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class AdjustCustomerCreditLimitUseCase(
        ICustomerCreditLimitRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(string taxId, decimal value, bool increase)
        {
            var entity = await repository.GetByTaxIdAsync(taxId)
                ?? throw new InvalidOperationException("Customer credit limit not found.");

            var now = DateTime.UtcNow;
            if (increase)
                entity.DecreaseUsedLimit(value, now, userContext.UserId);
            else
                entity.IncreaseUsedLimit(value, now, userContext.UserId); 

            await repository.UpdateAsync(entity);

            return new ServiceResult<string>("Customer credit limit adjusted.");
        }
    }
}
