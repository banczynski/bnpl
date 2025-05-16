using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class GetCreditAnalysisConfigByPartnerUseCase(ICreditAnalysisConfigurationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<CreditAnalysisConfigDto>>> ExecuteAsync(Guid partnerId)
        {
            var list = await repository.GetAllByPartnerAsync(partnerId);
            
            return new ServiceResult<IEnumerable<CreditAnalysisConfigDto>>(
                list.Select(c => c.ToDto()),
                ["Partner credit analysis configurations retrieved."]
            );
        }
    }
}
