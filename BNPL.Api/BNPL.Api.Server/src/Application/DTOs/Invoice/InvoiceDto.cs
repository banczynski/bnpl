using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Invoice
{
    public sealed record InvoiceDto(
        Guid Id,
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        DateTime DueDate,
        decimal TotalAmount,
        InvoiceStatus Status,
        bool IsIndividual,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        Guid CreatedBy,
        Guid UpdatedBy
    );
}
