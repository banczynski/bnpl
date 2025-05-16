using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class GetFinancialChargesByAffiliateUseCase(IFinancialChargesConfigurationRepository repository)
    {
        public async Task<ServiceResult<FinancialChargesConfigDto>> ExecuteAsync(Guid affiliateId)
        {
            var config = await repository.GetByAffiliateAsync(affiliateId)
                ?? throw new InvalidOperationException("Configuration not found.");

            return new ServiceResult<FinancialChargesConfigDto>(config.ToDto());
        }
    }
}
