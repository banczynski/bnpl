using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Installment
{
    public sealed record InstallmentDto(
        Guid Id,
        Guid PartnerId,
        Guid AffiliateId,
        Guid? ProposalId,
        Guid CustomerId,
        string CustomerTaxId,
        int Sequence,
        DateTime DueDate,
        decimal Amount,
        InstallmentStatus Status,
        Guid? InvoiceId,
        Guid? PaymentId
    );
}
