using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class InactivateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid partnerId, Guid? affiliateId)
        {
            await repository.InactivateAsync(partnerId, affiliateId, userContext.UserId, DateTime.UtcNow);
            return new ServiceResult<string>("Configuration inactivated.");
        }
    }
}
