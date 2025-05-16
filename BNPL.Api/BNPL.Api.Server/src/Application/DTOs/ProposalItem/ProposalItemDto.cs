namespace BNPL.Api.Server.src.Application.DTOs.ProposalItem
{
    public sealed record ProposalItemDto(
        Guid ProposalId,
        Guid ProductId,
        string Description,
        decimal Amount,
        Guid AffiliateId,
        bool Returned,
        string? ReturnReason,
        DateTime? ReturnedAt
    );
}
