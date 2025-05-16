using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class InactivateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid partnerId, Guid? affiliateId)
        {
            var now = DateTime.UtcNow;

            await repository.InactivateAsync(partnerId, affiliateId, userContext.UserId, now);

            return new ServiceResult<string>("Configuration inactivated.");
        }
    }
}
