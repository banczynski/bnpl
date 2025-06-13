namespace BNPL.Api.Server.src.Application.DTOs.CreditLimit
{
    public sealed record CustomerCreditLimitDto(
        Guid Code,
        Guid PartnerId,
        Guid AffiliateId,
        string CustomerTaxId,
        decimal ApprovedLimit,
        decimal UsedLimit,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
