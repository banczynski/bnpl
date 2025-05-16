namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record CreateInvoiceRequest(
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        DateTime DueDate,
        decimal TotalAmount,
        List<Guid> InstallmentIds
    );
}
