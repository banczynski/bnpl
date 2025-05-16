namespace BNPL.Api.Server.src.Application.DTOs.ProposalItem
{
    public sealed record CreateProposalItemRequest(
        Guid ProductId,
        string Description,
        decimal Amount,
        Guid AffiliateId
    );
}
