namespace BNPL.Api.Server.src.Application.DTOs.Affiliate
{
    public sealed record UpdateAffiliateRequest(
        string Name,
        string TaxId
    );
}
