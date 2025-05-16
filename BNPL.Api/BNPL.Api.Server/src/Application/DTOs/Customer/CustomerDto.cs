namespace BNPL.Api.Server.src.Application.DTOs.Customer
{
    public sealed record CustomerDto(
        Guid Id,
        Guid PartnerId,
        Guid AffiliateId,
        string TaxId,
        string Name,
        string Email,
        string Phone,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
