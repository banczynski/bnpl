using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.UseCases.ProposalItem;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProposalItemController(
        GetProposalItemsByProposalIdUseCase getUseCase,
        IUseCase<MarkProposalItemAsReturnedRequestUseCase, Result<bool, Error>> returnUseCase,
        IUseCase<ConfirmProposalItemReturnRequestUseCase, Result<bool, Error>> confirmReturnUseCase,
        IUseCase<CreateProposalItemRequestUseCase, Result<ProposalItemDto, Error>> createUseCase,
        IUseCase<CreateProposalItemsRequestUseCase, Result<IEnumerable<ProposalItemDto>, Error>> createManyUseCase
    ) : ControllerBase
    {
        [HttpGet("by-proposal/{proposalId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalItemDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ProposalItemDto>, Error>>> GetByProposal(Guid proposalId)
        {
            var result = await getUseCase.ExecuteAsync(proposalId);
            return Ok(result);
        }

        [HttpPost("{itemId:guid}/return")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> ReturnItem(
            Guid itemId,
            [FromBody] ReturnProposalItemRequest request)
        {
            var useCaseRequest = new MarkProposalItemAsReturnedRequestUseCase(itemId, request.Reason);
            var result = await returnUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("confirm-return/{itemId:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> ConfirmReturn(Guid itemId)
        {
            var useCaseRequest = new ConfirmProposalItemReturnRequestUseCase(itemId);
            var result = await confirmReturnUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{proposalId:guid}/item")]
        [ProducesResponseType(typeof(Result<ProposalItemDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProposalItemDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<ProposalItemDto, Error>>> AddItem(
            Guid proposalId,
            [FromBody] CreateProposalItemRequest request)
        {
            var useCaseRequest = new CreateProposalItemRequestUseCase(proposalId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{proposalId:guid}/items")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalItemDto>, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalItemDto>, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<IEnumerable<ProposalItemDto>, Error>>> AddItems(
            Guid proposalId,
            [FromBody] CreateProposalItemsRequest request)
        {
            var useCaseRequest = new CreateProposalItemsRequestUseCase(proposalId, request);
            var result = await createManyUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}