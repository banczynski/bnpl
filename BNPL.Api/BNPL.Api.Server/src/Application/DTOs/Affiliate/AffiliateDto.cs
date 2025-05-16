namespace BNPL.Api.Server.src.Application.DTOs.Affiliate
{
    public sealed record AffiliateDto(
        Guid Id,
        Guid PartnerId,
        string Name,
        string TaxId,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
