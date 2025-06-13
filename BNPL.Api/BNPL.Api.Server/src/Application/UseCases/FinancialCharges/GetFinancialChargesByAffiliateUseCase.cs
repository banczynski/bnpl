using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class GetFinancialChargesByAffiliateUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository)
    {
        public async Task<Result<FinancialChargesConfigDto, Error>> ExecuteAsync(Guid affiliateId)
        {
            var config = await financialChargesConfigurationRepository.GetByAffiliateAsync(affiliateId);

            if (config is null)
                return Result<FinancialChargesConfigDto, Error>.Fail(DomainErrors.FinancialCharges.ConfigNotFound);

            return Result<FinancialChargesConfigDto, Error>.Ok(config.ToDto());
        }
    }
}