namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public sealed record CreateFinancialChargesConfigRequest(
        decimal InterestRate,
        decimal ChargesRate,
        decimal LateFeeRate,
        int GraceDays,
        bool ApplyCompoundInterest
    );
}
