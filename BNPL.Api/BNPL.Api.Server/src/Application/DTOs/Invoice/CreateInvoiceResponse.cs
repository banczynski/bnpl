namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record CreateInvoiceResponse(
        Guid Id,
        DateTime DueDate,
        decimal TotalAmount
    );
}
