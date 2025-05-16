using BNPL.Api.Server.src.Application.DTOs.ProposalItem;

namespace BNPL.Api.Server.src.Application.DTOs.Proposal
{
    public sealed class ProposalWithItemsDto
    {
        public ProposalDto Proposal { get; set; } = default!;
        public List<ProposalItemDto> Items { get; set; } = [];
    }
}
