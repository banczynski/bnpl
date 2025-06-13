namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record PublicCreditAnalysisResult(
        decimal ApprovedLimit,
        int MaxInstallments,
        decimal MonthlyInterestRate
    );
}
