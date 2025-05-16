namespace BNPL.Api.Server.src.Application.DTOs.Customer
{
    public sealed record UpdateCustomerRequest(
        string Name,
        string Email,
        string Phone
    );
}
