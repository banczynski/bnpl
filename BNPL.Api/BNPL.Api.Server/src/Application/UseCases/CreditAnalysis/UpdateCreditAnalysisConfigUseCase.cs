using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class UpdateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository creditAnalysisRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CreditAnalysisConfigDto, string>> ExecuteAsync(Guid partnerId, Guid? affiliateId, UpdateCreditAnalysisConfigRequest request)
        {
            var entity = (affiliateId is not null
                ? await creditAnalysisRepository.GetByAffiliateAsync(affiliateId.Value)
                : await creditAnalysisRepository.GetByPartnerAsync(partnerId));

            if (entity is null)
                return Result<CreditAnalysisConfigDto, string>.Fail("Configuration not found.");

            entity.UpdateEntity(request, DateTime.UtcNow, userContext.GetRequiredUserId());
            await creditAnalysisRepository.UpdateAsync(entity);

            return Result<CreditAnalysisConfigDto, string>.Ok(entity.ToDto());
        }
    }
}
