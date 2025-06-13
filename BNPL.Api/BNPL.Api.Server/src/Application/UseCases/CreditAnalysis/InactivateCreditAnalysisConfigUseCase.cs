using Core.Context.Interfaces;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class InactivateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository creditAnalysisRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid partnerId, Guid? affiliateId)
        {
            await creditAnalysisRepository.InactivateAsync(partnerId, affiliateId, userContext.GetRequiredUserId(), DateTime.UtcNow);
            return Result<bool, string>.Ok(true);
        }
    }
}
