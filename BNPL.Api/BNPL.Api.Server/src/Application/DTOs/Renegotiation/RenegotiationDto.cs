using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Renegotiation
{
    public sealed record RenegotiationDto(
        Guid Id,
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        List<Guid> OriginalInvoiceIds,
        List<Guid> OriginalInstallmentIds,
        decimal OriginalTotalAmount,
        decimal NewTotalAmount,
        int NewInstallments,
        decimal MonthlyInterestRate,
        RenegotiationStatus Status,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
