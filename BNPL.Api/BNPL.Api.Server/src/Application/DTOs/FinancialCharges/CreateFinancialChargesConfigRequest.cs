namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public sealed record CreateFinancialChargesConfigRequest(
        Guid PartnerId,
        Guid? AffiliateId,
        decimal InterestRate,
        decimal ChargesRate,
        decimal LateFeeRate,
        int GraceDays,
        bool ApplyCompoundInterest
    );
}
