namespace BNPL.Api.Server.src.Application.DTOs.ProposalItem
{
    public sealed record ReturnProposalItemRequest(
        Guid ProductId,
        string Reason
    );
}
