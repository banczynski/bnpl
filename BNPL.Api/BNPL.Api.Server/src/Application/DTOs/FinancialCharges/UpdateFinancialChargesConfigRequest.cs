namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public sealed record UpdateFinancialChargesConfigRequest(
        decimal InterestRate,
        decimal ChargesRate,
        decimal LateFeeRate,
        int GraceDays,
        bool ApplyCompoundInterest
    );
}
