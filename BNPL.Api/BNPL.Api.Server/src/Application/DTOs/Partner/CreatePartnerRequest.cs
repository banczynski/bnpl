namespace BNPL.Api.Server.src.Application.DTOs.Partner
{
    public sealed record CreatePartnerRequest(
        string Name,
        string TaxId
    );
}
