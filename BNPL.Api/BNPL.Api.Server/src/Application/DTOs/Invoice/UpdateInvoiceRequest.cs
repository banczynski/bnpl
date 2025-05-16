namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record UpdateInvoiceRequest(
        DateTime DueDate,
        decimal TotalAmount
    );
}
