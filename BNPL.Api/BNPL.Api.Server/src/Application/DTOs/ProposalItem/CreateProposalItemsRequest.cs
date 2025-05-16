namespace BNPL.Api.Server.src.Application.DTOs.ProposalItem
{
    public sealed record CreateProposalItemsRequest(
        List<CreateProposalItemRequest> Items
    );
}
