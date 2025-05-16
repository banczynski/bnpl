namespace BNPL.Api.Server.src.Application.DTOs.Installment
{
    public sealed record InstallmentChargesInput(
        decimal OriginalAmount,
        DateTime DueDate,
        DateTime PaymentDate,
        decimal DailyInterestRate,
        decimal FixedChargesRate 
    );
}
