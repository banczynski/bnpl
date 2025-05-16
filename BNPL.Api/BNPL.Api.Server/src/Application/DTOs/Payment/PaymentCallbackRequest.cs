namespace BNPL.Api.Server.src.Application.DTOs.Payment
{
    public sealed record PaymentCallbackRequest(
        Guid InvoiceId,
        decimal PaidAmount,
        DateTime? PaymentDate = null
    );
}
