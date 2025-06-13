using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed record InactivateCreditAnalysisConfigRequestUseCase(Guid PartnerId, Guid? AffiliateId);

    public sealed class InactivateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository creditAnalysisRepository,
        IUserContext userContext
    ) : IUseCase<InactivateCreditAnalysisConfigRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivateCreditAnalysisConfigRequestUseCase request)
        {
            var success = await creditAnalysisRepository.InactivateByPartnerOrAffiliateAsync(
                request.PartnerId,
                request.AffiliateId,
                userContext.GetRequiredUserId()
            );

            if (!success)
                return Result<bool, Error>.Fail(DomainErrors.CreditAnalysis.ConfigNotFound);

            return Result<bool, Error>.Ok(true);
        }
    }
}