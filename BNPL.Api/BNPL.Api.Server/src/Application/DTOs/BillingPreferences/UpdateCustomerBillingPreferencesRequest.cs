namespace BNPL.Api.Server.src.Application.DTOs.BillingPreferences
{
    public sealed record UpdateCustomerBillingPreferencesRequest(
        int InvoiceDueDay,
        bool ConsolidatedInvoiceEnabled = false
    );
}
