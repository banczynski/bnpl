namespace BNPL.Api.Server.src.Application.DTOs.Customer
{
    public sealed record CreateCustomerRequest(
        string TaxId,
        string Name,
        string Email,
        string Phone,
        bool SkipKyc
    );
}
