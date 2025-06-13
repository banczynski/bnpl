namespace BNPL.Api.Server.src.Application.DTOs.Affiliate
{
    public sealed record CreateAffiliateRequest(
        string Name,
        string TaxId
    );
}
