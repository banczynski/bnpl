namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public sealed record FinancialChargesConfigDto(
        Guid Id,
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
        Guid CreatedBy,
        Guid UpdatedBy
    );
}
