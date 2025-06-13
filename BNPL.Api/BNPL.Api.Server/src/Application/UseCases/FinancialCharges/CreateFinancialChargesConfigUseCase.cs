using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class CreateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<FinancialChargesConfigDto, string>> ExecuteAsync(Guid partnerId, Guid? affiliateId, CreateFinancialChargesConfigRequest request)
        {
            var entity = request.ToEntity(partnerId, affiliateId, userContext.GetRequiredUserId());
            await financialChargesConfigurationRepository.InsertAsync(entity);

            return Result<FinancialChargesConfigDto, string>.Ok(entity.ToDto());
        }
    }
}
