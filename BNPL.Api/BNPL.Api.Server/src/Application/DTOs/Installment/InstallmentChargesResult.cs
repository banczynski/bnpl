namespace BNPL.Api.Server.src.Application.DTOs.Installment
{
    public sealed record InstallmentChargesResult(
        int DaysLate,
        decimal FixedCharges,
        decimal InterestAmount,
        decimal TotalWithCharges
    );
}
