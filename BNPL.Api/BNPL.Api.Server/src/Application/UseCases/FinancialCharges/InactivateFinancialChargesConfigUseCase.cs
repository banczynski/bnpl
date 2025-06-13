using Core.Context.Interfaces;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class InactivateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid partnerId, Guid? affiliateId)
        {
            var now = DateTime.UtcNow;

            await financialChargesConfigurationRepository.InactivateAsync(partnerId, affiliateId, userContext.GetRequiredUserId(), now);

            return Result<bool, string>.Ok(true);
        }
    }
}
