using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class GetFinancialChargesByPartnerUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository)
    {
        public async Task<Result<IEnumerable<FinancialChargesConfigDto>, Error>> ExecuteAsync(Guid partnerId)
        {
            var list = await financialChargesConfigurationRepository.GetAllByPartnerAsync(partnerId);
            return Result<IEnumerable<FinancialChargesConfigDto>, Error>.Ok(list.Select(c => c.ToDto()));
        }
    }
}