namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public sealed record FinancialChargesConfigDto(
        Guid PartnerId,
        Guid? AffiliateId,
        decimal InterestRate,
        decimal ChargesRate,
        decimal LateFeeRate,
        int GraceDays,
        bool ApplyCompoundInterest,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
