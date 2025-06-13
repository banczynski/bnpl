namespace BNPL.Api.Server.src.Application.DTOs.ProposalItem
{
    public sealed record CreateProposalItemRequest(
        string Description,
        decimal Amount
    );
}
