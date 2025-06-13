using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.BillingPreferences;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.BillingPreferences
{
    public sealed record UpdateCustomerBillingPreferencesRequestUseCase(
        Guid CustomerId,
        Guid AffiliateId,
        UpdateCustomerBillingPreferencesRequest Dto
    );

    public sealed class UpdateCustomerBillingPreferencesUseCase(
        ICustomerBillingPreferencesRepository billingPreferencesRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateCustomerBillingPreferencesRequestUseCase, Result<CustomerBillingPreferencesDto, Error>>
    {
        public async Task<Result<CustomerBillingPreferencesDto, Error>> ExecuteAsync(UpdateCustomerBillingPreferencesRequestUseCase useCaseRequest)
        {
            var (customerId, affiliateId, request) = useCaseRequest;

            if (request.InvoiceDueDay is < 1 or > 28)
                return Result<CustomerBillingPreferencesDto, Error>.Fail(DomainErrors.Billing.InvalidDueDay);

            var preferences = await billingPreferencesRepository.GetByCustomerIdAndAffiliateIdAsync(customerId, affiliateId, unitOfWork.Transaction);
            if (preferences is null)
                return Result<CustomerBillingPreferencesDto, Error>.Fail(DomainErrors.Billing.NotFound);

            preferences.InvoiceDueDay = request.InvoiceDueDay;
            preferences.ConsolidatedInvoiceEnabled = request.ConsolidatedInvoiceEnabled;
            preferences.UpdatedAt = DateTime.UtcNow;
            preferences.UpdatedBy = userContext.GetRequiredUserId();

            await billingPreferencesRepository.UpdateAsync(preferences, unitOfWork.Transaction);

            return Result<CustomerBillingPreferencesDto, Error>.Ok(preferences.ToDto());
        }
    }
}