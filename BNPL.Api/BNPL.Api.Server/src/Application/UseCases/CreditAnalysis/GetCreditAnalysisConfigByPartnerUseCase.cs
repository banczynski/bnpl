using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class GetCreditAnalysisConfigByPartnerUseCase(ICreditAnalysisConfigurationRepository creditAnalysisRepository)
    {
        public async Task<Result<IEnumerable<CreditAnalysisConfigDto>, string>> ExecuteAsync(Guid partnerId)
        {
            var list = await creditAnalysisRepository.GetAllByPartnerAsync(partnerId);

            return Result<IEnumerable<CreditAnalysisConfigDto>, string>.Ok(list.Select(c => c.ToDto()));
        }
    }
}
