namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreateCreditAnalysisConfigRequest(
        decimal MinApprovedPercentage,
        decimal MaxApprovedPercentage,
        decimal RejectionThreshold,
        decimal MaxCreditAmount,
        int MaxInstallments,
        decimal MonthlyInterestRate
    );
}
