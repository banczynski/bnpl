namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record UpdateCreditAnalysisConfigRequest(
        decimal MinApprovedPercentage,
        decimal MaxApprovedPercentage,
        decimal RejectionThreshold
    );
}
