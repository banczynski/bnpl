using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.UseCases.Proposal;
using BNPL.Api.Server.src.Application.UseCases.Signature;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProposalController(
        IUseCase<CreateProposalRequestUseCase, Result<CreateProposalResponse, Error>> createUseCase,
        IUseCase<InactivateProposalRequestUseCase, Result<bool, Error>> inactivateUseCase,
        IUseCase<CancelProposalRequestUseCase, Result<bool, Error>> cancelUseCase,
        IUseCase<CancelProposalsRequestUseCase, Result<int, Error>> cancelProposalsUseCase,
        GetProposalByIdUseCase getByIdUseCase,
        GetProposalWithItemsUseCase getWithItemsUseCase,
        GetProposalsByCustomerIdUseCase getByCustomerUseCase,
        GetProposalsByAffiliateIdUseCase getByAffiliateUseCase,
        GetProposalsByPartnerIdUseCase getByPartnerUseCase,
        IUseCase<GenerateSignatureTokenRequestUseCase, Result<SignatureTokenResponse, Error>> generateSignatureTokenUseCase,
        IUseCase<MarkProposalAsFinalizedRequestUseCase, Result<bool, Error>> markAsFinalizedUseCase,
        GenerateFinalContractUseCase generateFinalContractUseCase
    ) : ControllerBase
    {
        [HttpPost("{simulationId:guid}")]
        [ProducesResponseType(typeof(Result<CreateProposalResponse, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CreateProposalResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreateProposalResponse, Error>>> Create(
            Guid simulationId,
            [FromQuery] int term)
        {
            var useCaseRequest = new CreateProposalRequestUseCase(simulationId, term);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetSuccess(out var successValue))
                return CreatedAtAction(nameof(GetById), new { id = successValue.Id }, result);

            return BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Inactivate(Guid id)
        {
            var useCaseRequest = new InactivateProposalRequestUseCase(id);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<ProposalDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProposalDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<ProposalDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id:guid}/with-items")]
        [ProducesResponseType(typeof(Result<ProposalWithItemsDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProposalWithItemsDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<ProposalWithItemsDto, Error>>> GetWithItems(Guid id)
        {
            var result = await getWithItemsUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Cancel(Guid id)
        {
            var useCaseRequest = new CancelProposalRequestUseCase(id);
            var result = await cancelUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("cancel")]
        [ProducesResponseType(typeof(Result<int, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<int, Error>>> Cancel()
        {
            var useCaseRequest = new CancelProposalsRequestUseCase();
            var result = await cancelProposalsUseCase.ExecuteAsync(useCaseRequest);
            return Ok(result);
        }

        [HttpGet("by-customer/{customerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, Error>>> GetByCustomer(Guid customerId)
        {
            var result = await getByCustomerUseCase.ExecuteAsync(customerId);
            return Ok(result);
        }

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, Error>>> GetByAffiliate(Guid affiliateId)
        {
            var result = await getByAffiliateUseCase.ExecuteAsync(affiliateId);
            return Ok(result);
        }

        [HttpGet("by-partner/{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<ProposalDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, Error>>> GetByPartner(Guid partnerId)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId);
            return Ok(result);
        }

        [HttpPost("{id:guid}/generate-signature-link")]
        [ProducesResponseType(typeof(Result<SignatureTokenResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<SignatureTokenResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<SignatureTokenResponse, Error>>> GenerateSignatureLink(Guid id)
        {
            var useCaseRequest = new GenerateSignatureTokenRequestUseCase(id);
            var result = await generateSignatureTokenUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/finalize")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Finalize(Guid id)
        {
            var useCaseRequest = new MarkProposalAsFinalizedRequestUseCase(id);
            var result = await markAsFinalizedUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/generate-contract")]
        [ProducesResponseType(typeof(Result<Uri, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Uri, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<Uri, Error>>> GenerateContract(Guid id)
        {
            var result = await generateFinalContractUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}