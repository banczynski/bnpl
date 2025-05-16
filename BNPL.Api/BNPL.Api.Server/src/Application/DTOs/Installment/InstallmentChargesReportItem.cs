namespace BNPL.Api.Server.src.Application.DTOs.Installment
{
    public sealed record InstallmentChargesReportItem(
        Guid InstallmentId,
        Guid PartnerId,
        Guid AffiliateId,
        string CustomerTaxId,
        int Sequence,
        DateTime DueDate,
        int DaysLate,
        decimal OriginalAmount,
        decimal FixedCharges,
        decimal InterestAmount,
        decimal TotalWithCharges
    );
}
