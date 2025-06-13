using BNPL.Api.Server.src.Application.DTOs.Proposal;
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
        CreateProposalUseCase createUseCase,
        InactivateProposalUseCase inactivateUseCase,
        CancelProposalUseCase cancelUseCase,
        CancelProposalsUseCase cancelProposalsUseCase,
        GetProposalByIdUseCase getByIdUseCase,
        GetProposalWithItemsUseCase getWithItemsUseCase,
        GetProposalsByCustomerIdUseCase getByCustomerUseCase,
        GetProposalsByAffiliateIdUseCase getByAffiliateUseCase,
        GetProposalsByPartnerIdUseCase getByPartnerUseCase,
        GenerateSignatureTokenUseCase generateSignatureLinkUseCase,
        MarkProposalAsFinalizedUseCase markAsFinalizedUseCase,
        MarkProposalAsApprovedUseCase markAsApprovedUseCase,
        MarkProposalAsActiveUseCase markAsActiveUseCase,
        GenerateFinalContractUseCase generateFinalContractUseCase
    ) : ControllerBase
    {
        [HttpPost("{simulationId:guid}")]
        public async Task<ActionResult<Result<CreateProposalResponse, string[]>>> Create(
            Guid simulationId,
            [FromQuery] int term)
        {
            var result = await createUseCase.ExecuteAsync(simulationId, term);

            if (result is Result<CreateProposalResponse, string>.Success success)
                return CreatedAtAction(nameof(GetById), new { id = success.Value.Id }, result);

            return BadRequest(result);
        }

        [HttpPost("{proposalId:guid}/approve")]
        public async Task<ActionResult<Result<string, string>>> MarkAsApproved(Guid proposalId)
            => Ok(await markAsApprovedUseCase.ExecuteAsync(proposalId));

        [HttpPost("{proposalId:guid}/activate")]
        public async Task<ActionResult<Result<string, string>>> MarkAsActive(Guid proposalId)
            => Ok(await markAsActiveUseCase.ExecuteAsync(proposalId));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result<string, string[]>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<ProposalDto, string[]>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}/with-items")]
        public async Task<ActionResult<Result<ProposalWithItemsDto, string[]>>> GetWithItems(Guid id)
            => Ok(await getWithItemsUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/cancel")]
        public async Task<ActionResult<Result<string, string[]>>> Cancel(Guid id)
            => Ok(await cancelUseCase.ExecuteAsync(id));

        // TODO Esse processo deverá ser disparado por um cron
        [HttpPost("cancel")]
        public async Task<ActionResult<Result<int, string[]>>> Cancel()
            => Ok(await cancelProposalsUseCase.ExecuteAsync());

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, string[]>>> GetByCustomer(Guid customerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId, onlyActive));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, string[]>>> GetByAffiliate(Guid affiliateId, [FromQuery] bool onlyActive = true)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId, onlyActive));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<ProposalDto>, string[]>>> GetByPartner(Guid partnerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive));

        [HttpPost("{id:guid}/generate-signature-link")]
        public async Task<ActionResult<Result<Uri, string[]>>> GenerateSignatureLink(Guid id)
            => Ok(await generateSignatureLinkUseCase.ExecuteAsync(id));

        // TODO Esse processo deverá ser disparado por um cron
        [HttpPost("{id:guid}/finalize")]
        public async Task<ActionResult<Result<string, string[]>>> Finalize(Guid id)
            => Ok(await markAsFinalizedUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/generate-contract")]
        public async Task<ActionResult<Result<Uri, string[]>>> GenerateContract(Guid id)
            => Ok(await generateFinalContractUseCase.ExecuteAsync(id));
    }
}
