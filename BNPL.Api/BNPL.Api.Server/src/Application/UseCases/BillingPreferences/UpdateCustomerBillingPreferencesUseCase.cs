using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.BillingPreferences;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.BillingPreferences
{
    public sealed class UpdateCustomerBillingPreferencesUseCase(
        ICustomerBillingPreferencesRepository billingPreferencesRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CustomerBillingPreferencesDto, string>> ExecuteAsync(Guid customerId, Guid affiliateId, UpdateCustomerBillingPreferencesRequest request)
        {
            if (request.InvoiceDueDay is < 1 or > 28)
                return Result<CustomerBillingPreferencesDto, string>.Fail("Invoice due day must be between 1 and 28.");

            var preferences = await billingPreferencesRepository.GetByCustomerIdAndAffiliateIdAsync(customerId, affiliateId);
            if (preferences is null)
                return Result<CustomerBillingPreferencesDto, string>.Fail("Billing preferences not found.");

            preferences.InvoiceDueDay = request.InvoiceDueDay;
            preferences.ConsolidatedInvoiceEnabled = request.ConsolidatedInvoiceEnabled;
            preferences.UpdatedAt = DateTime.UtcNow;
            preferences.UpdatedBy = userContext.GetRequiredUserId();

            await billingPreferencesRepository.UpdateAsync(preferences);

            return Result<CustomerBillingPreferencesDto, string>.Ok(preferences.ToDto());
        }
    }
}
