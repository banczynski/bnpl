namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record PaymentLinkRequest(
        Guid InvoiceId,
        string CustomerTaxId,
        decimal Amount,
        DateTime DueDate
    );
}
