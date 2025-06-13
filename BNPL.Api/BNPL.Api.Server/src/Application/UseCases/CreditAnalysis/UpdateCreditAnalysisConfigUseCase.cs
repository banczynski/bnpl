using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed record UpdateCreditAnalysisConfigRequestUseCase(Guid PartnerId, Guid? AffiliateId, UpdateCreditAnalysisConfigRequest Dto);

    public sealed class UpdateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository creditAnalysisRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateCreditAnalysisConfigRequestUseCase, Result<CreditAnalysisConfigDto, Error>>
    {
        public async Task<Result<CreditAnalysisConfigDto, Error>> ExecuteAsync(UpdateCreditAnalysisConfigRequestUseCase request)
        {
            var entity = (request.AffiliateId is not null
                ? await creditAnalysisRepository.GetByAffiliateAsync(request.AffiliateId.Value, unitOfWork.Transaction)
                : await creditAnalysisRepository.GetByPartnerAsync(request.PartnerId, unitOfWork.Transaction));

            if (entity is null)
                return Result<CreditAnalysisConfigDto, Error>.Fail(DomainErrors.CreditAnalysis.ConfigNotFound);

            entity.UpdateEntity(request.Dto, DateTime.UtcNow, userContext.GetRequiredUserId());
            await creditAnalysisRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<CreditAnalysisConfigDto, Error>.Ok(entity.ToDto());
        }
    }
}