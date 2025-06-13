using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class GetCreditAnalysisConfigByAffiliateUseCase(ICreditAnalysisConfigurationRepository creditAnalysisRepository)
    {
        public async Task<Result<CreditAnalysisConfigDto, string>> ExecuteAsync(Guid affiliateId)
        {
            var entity = await creditAnalysisRepository.GetByAffiliateAsync(affiliateId);

            if (entity is null)
                return Result<CreditAnalysisConfigDto, string>.Fail("Configuration not found.");

            return Result<CreditAnalysisConfigDto, string>.Ok(entity.ToDto());
        }
    }
}
