namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record GeneratePaymentLinkResponse(
        Guid InvoiceId,
        decimal OriginalAmount,
        decimal ChargesAmount,
        decimal FinalAmount,
        Uri PaymentLink
    );
}
