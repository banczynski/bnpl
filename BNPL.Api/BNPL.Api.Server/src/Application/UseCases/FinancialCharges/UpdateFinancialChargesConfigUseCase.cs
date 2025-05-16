using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class UpdateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid partnerId, Guid? affiliateId, UpdateFinancialChargesConfigRequest request)
        {
            var entity = (affiliateId is not null
                ? await repository.GetByAffiliateAsync(affiliateId.Value)
                : await repository.GetByPartnerAsync(partnerId)) 
                ?? throw new InvalidOperationException("Configuration not found.");

            var now = DateTime.UtcNow;
            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<string>("Configuration updated.");
        }
    }
}
