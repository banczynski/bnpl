using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed record InactivateFinancialChargesConfigRequestUseCase(Guid PartnerId, Guid? AffiliateId);

    public sealed class InactivateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository financialChargesConfigurationRepository,
        IUserContext userContext
    ) : IUseCase<InactivateFinancialChargesConfigRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivateFinancialChargesConfigRequestUseCase request)
        {
            var success = await financialChargesConfigurationRepository.InactivateByPartnerOrAffiliateAsync(
                request.PartnerId,
                request.AffiliateId,
                userContext.GetRequiredUserId()
            );

            if (!success)
                return Result<bool, Error>.Fail(DomainErrors.FinancialCharges.ConfigNotFound);

            return Result<bool, Error>.Ok(true);
        }
    }
}