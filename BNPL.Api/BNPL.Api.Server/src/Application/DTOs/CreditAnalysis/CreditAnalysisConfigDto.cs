namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreditAnalysisConfigDto(
        Guid PartnerId,
        Guid? AffiliateId,
        decimal MinApprovedPercentage,
        decimal MaxApprovedPercentage,
        decimal RejectionThreshold,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
