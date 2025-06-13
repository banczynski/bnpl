using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed record CreateFinancialChargesConfigRequestUseCase(Guid PartnerId, Guid? AffiliateId, CreateFinancialChargesConfigRequest Dto);

    public sealed class CreateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>>
    {
        public async Task<Result<FinancialChargesConfigDto, Error>> ExecuteAsync(CreateFinancialChargesConfigRequestUseCase request)
        {
            var entity = request.Dto.ToEntity(request.PartnerId, request.AffiliateId, userContext.GetRequiredUserId());
            await financialChargesConfigurationRepository.InsertAsync(entity, unitOfWork.Transaction);

            return Result<FinancialChargesConfigDto, Error>.Ok(entity.ToDto());
        }
    }
}