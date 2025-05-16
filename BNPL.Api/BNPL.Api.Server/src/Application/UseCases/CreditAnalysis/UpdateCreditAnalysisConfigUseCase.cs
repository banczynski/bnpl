using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class UpdateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid partnerId, Guid? affiliateId, UpdateCreditAnalysisConfigRequest request)
        {
            var entity = (affiliateId is not null
                ? await repository.GetByAffiliateAsync(affiliateId.Value)
                : await repository.GetByPartnerAsync(partnerId)) 
                ?? throw new InvalidOperationException("Configuration not found.");

            entity.UpdateEntity(request, DateTime.UtcNow, userContext.UserId);
            await repository.UpdateAsync(entity);

            return new ServiceResult<string>("Configuration updated.");
        }
    }
}
