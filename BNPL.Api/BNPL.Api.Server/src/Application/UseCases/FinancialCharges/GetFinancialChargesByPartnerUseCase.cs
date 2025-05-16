using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class GetFinancialChargesByPartnerUseCase(IFinancialChargesConfigurationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<FinancialChargesConfigDto>>> ExecuteAsync(Guid partnerId)
        {
            var list = await repository.GetAllByPartnerAsync(partnerId);
            return new ServiceResult<IEnumerable<FinancialChargesConfigDto>>(list.Select(c => c.ToDto()));
        }
    }
}
