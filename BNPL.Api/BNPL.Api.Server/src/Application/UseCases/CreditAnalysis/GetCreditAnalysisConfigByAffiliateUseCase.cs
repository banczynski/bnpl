using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class GetCreditAnalysisConfigByAffiliateUseCase(ICreditAnalysisConfigurationRepository repository)
    {
        public async Task<ServiceResult<CreditAnalysisConfigDto>> ExecuteAsync(Guid affiliateId)
        {
            var entity = await repository.GetByAffiliateAsync(affiliateId)
                ?? throw new InvalidOperationException("Configuration not found.");

            return new ServiceResult<CreditAnalysisConfigDto>(
                entity.ToDto(),
                ["Affiliate credit analysis configuration retrieved."]
            );
        }
    }
}
