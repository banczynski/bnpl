namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreditAnalysisConfigDto(
        Guid Id,
        Guid PartnerId,
        Guid? AffiliateId,
        decimal MinApprovedPercentage,
        decimal MaxApprovedPercentage,
        decimal RejectionThreshold,
        decimal MaxCreditAmount,
        int MaxInstallments,
        decimal MonthlyInterestRate,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        Guid CreatedBy,
        Guid UpdatedBy
    );
}
