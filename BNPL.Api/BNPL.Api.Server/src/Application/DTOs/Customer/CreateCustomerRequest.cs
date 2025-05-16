namespace BNPL.Api.Server.src.Application.DTOs.Customer
{
    public sealed record CreateCustomerRequest(
        Guid PartnerId,
        Guid AffiliateId,
        string TaxId,
        string Name,
        string Email,
        string Phone,
        bool SkipKyc
    );
}
