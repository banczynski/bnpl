using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed record UpdateFinancialChargesConfigRequestUseCase(Guid PartnerId, Guid? AffiliateId, UpdateFinancialChargesConfigRequest Dto);

    public sealed class UpdateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>>
    {
        public async Task<Result<FinancialChargesConfigDto, Error>> ExecuteAsync(UpdateFinancialChargesConfigRequestUseCase request)
        {
            var entity = (request.AffiliateId is not null
                ? await financialChargesConfigurationRepository.GetByAffiliateAsync(request.AffiliateId.Value, unitOfWork.Transaction)
                : await financialChargesConfigurationRepository.GetByPartnerAsync(request.PartnerId, unitOfWork.Transaction));

            if (entity is null)
                return Result<FinancialChargesConfigDto, Error>.Fail(DomainErrors.FinancialCharges.ConfigNotFound);

            entity.UpdateEntity(request.Dto, DateTime.UtcNow, userContext.GetRequiredUserId());
            await financialChargesConfigurationRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<FinancialChargesConfigDto, Error>.Ok(entity.ToDto());
        }
    }
}