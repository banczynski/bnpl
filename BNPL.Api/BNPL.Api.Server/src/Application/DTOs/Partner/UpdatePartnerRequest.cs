namespace BNPL.Api.Server.src.Application.DTOs.Partner
{
    public sealed record UpdatePartnerRequest(
        string Name,
        string TaxId
    );
}
