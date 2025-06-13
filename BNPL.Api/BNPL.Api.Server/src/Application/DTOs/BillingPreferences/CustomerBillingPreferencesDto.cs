namespace BNPL.Api.Server.src.Application.DTOs.BillingPreferences
{
    public sealed record CustomerBillingPreferencesDto(
        Guid Code,
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        int InvoiceDueDay,
        bool ConsolidatedInvoiceEnabled,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}