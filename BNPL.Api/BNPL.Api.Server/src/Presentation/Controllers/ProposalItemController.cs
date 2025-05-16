using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.UseCases.ProposalItem;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProposalItemController(
        GetProposalItemsByProposalIdUseCase getUseCase,
        MarkProposalItemAsReturnedUseCase returnUseCase,
        CreateProposalItemUseCase createUseCase,
        CreateProposalItemsUseCase createManyUseCase
    ) : ControllerBase
    {
        [HttpGet("by-proposal/{proposalId:guid}")]
        public async Task<ActionResult<ServiceResult<List<ProposalItemDto>>>> GetByProposal(Guid proposalId)
            => Ok(await getUseCase.ExecuteAsync(proposalId));

        [HttpPost("{proposalId:guid}/return")]
        public async Task<ActionResult<ServiceResult<string>>> ReturnItem(Guid proposalId, [FromBody] ReturnProposalItemRequest request)
            => Ok(await returnUseCase.ExecuteAsync(proposalId, request.ProductId, request.Reason));

        [HttpPost("{proposalId:guid}/item")]
        public async Task<ActionResult<ServiceResult<string>>> AddItem(Guid proposalId, [FromBody] CreateProposalItemRequest request)
            => Ok(await createUseCase.ExecuteAsync(proposalId, request));

        [HttpPost("{proposalId:guid}/items")]
        public async Task<ActionResult<ServiceResult<string>>> AddItems(Guid proposalId, [FromBody] CreateProposalItemsRequest request)
            => Ok(await createManyUseCase.ExecuteAsync(proposalId, request));
    }
}
