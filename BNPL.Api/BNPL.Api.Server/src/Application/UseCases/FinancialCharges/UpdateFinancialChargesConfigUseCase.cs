using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class UpdateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<FinancialChargesConfigDto, string>> ExecuteAsync(Guid partnerId, Guid? affiliateId, UpdateFinancialChargesConfigRequest request)
        {
            var entity = (affiliateId is not null
                ? await financialChargesConfigurationRepository.GetByAffiliateAsync(affiliateId.Value)
                : await financialChargesConfigurationRepository.GetByPartnerAsync(partnerId));

            if (entity is null)
                return Result<FinancialChargesConfigDto, string>.Fail("Configuration not found.");

            entity.UpdateEntity(request, DateTime.UtcNow, userContext.GetRequiredUserId());
            await financialChargesConfigurationRepository.UpdateAsync(entity);

            return Result<FinancialChargesConfigDto, string>.Ok(entity.ToDto());
        }
    }
}
