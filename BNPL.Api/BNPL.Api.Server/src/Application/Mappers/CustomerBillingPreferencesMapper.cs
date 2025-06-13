using BNPL.Api.Server.src.Application.UseCases.BillingPreferences;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CustomerBillingPreferencesMapper
    {
        public static CustomerBillingPreferencesDto ToDto(this CustomerBillingPreferences entity)
        {
            return new CustomerBillingPreferencesDto(
                Code: entity.Code,
                PartnerId: entity.PartnerId,
                AffiliateId: entity.AffiliateId,
                CustomerId: entity.CustomerId,
                CustomerTaxId: entity.CustomerTaxId,
                InvoiceDueDay: entity.InvoiceDueDay,
                ConsolidatedInvoiceEnabled: entity.ConsolidatedInvoiceEnabled,
                CreatedAt: entity.CreatedAt,
                UpdatedAt: entity.UpdatedAt
            );
        }
    }
}
