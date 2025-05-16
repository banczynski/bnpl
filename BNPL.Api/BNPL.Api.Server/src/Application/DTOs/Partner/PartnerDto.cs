namespace BNPL.Api.Server.src.Application.DTOs.Partner
{
    public sealed record PartnerDto(
        Guid Id,
        string Name,
        string TaxId,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
