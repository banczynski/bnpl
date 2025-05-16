namespace BNPL.Api.Server.src.Application.DTOs.Affiliate
{
    public sealed record CreateAffiliateRequest(
        Guid PartnerId,
        string Name,
        string TaxId
    );
}
