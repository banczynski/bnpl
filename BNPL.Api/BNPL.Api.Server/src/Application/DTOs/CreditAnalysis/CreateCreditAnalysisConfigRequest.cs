namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreateCreditAnalysisConfigRequest(
        Guid PartnerId,
        Guid? AffiliateId,
        decimal MinApprovedPercentage,
        decimal MaxApprovedPercentage,
        decimal RejectionThreshold
    );
}
