namespace BNPL.Api.Server.src.Application.DTOs.CreditLimit
{
    public sealed record CreditLimitUpsertRequest(
        Guid PartnerId,
        Guid AffiliateId,
        string CustomerTaxId,
        decimal ApprovedLimit
    );
}
