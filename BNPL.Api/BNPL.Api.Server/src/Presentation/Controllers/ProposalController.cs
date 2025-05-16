using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.UseCases.Proposal;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProposalController(
        CreateProposalUseCase createUseCase,
        UpdateProposalUseCase updateUseCase,
        InactivateProposalUseCase inactivateUseCase,
        CancelProposalUseCase cancelUseCase,
        GetProposalByIdUseCase getByIdUseCase,
        GetProposalWithItemsUseCase getWithItemsUseCase,
        GetProposalsByCustomerIdUseCase getByCustomerUseCase,
        GetProposalsByAffiliateIdUseCase getByAffiliateUseCase,
        GetProposalsByPartnerIdUseCase getByPartnerUseCase, 
        GenerateSignatureLinkUseCase generateSignatureLinkUseCase,
        MarkProposalAsSignedUseCase markAsSignedUseCase,
        MarkProposalAsDisbursedUseCase markAsDisbursedUseCase,
        MarkProposalAsFinalizedUseCase markAsFinalizedUseCase,
        FormalizeProposalUseCase formalizeUseCase,
        GenerateFinalContractUseCase generateFinalContractUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<CreateProposalResponse>>> Create([FromBody] CreateProposalRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResult<ProposalDto>>> Update(Guid id, [FromBody] UpdateProposalRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<ProposalDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}/with-items")]
        public async Task<ActionResult<ServiceResult<ProposalWithItemsDto>>> GetWithItems(Guid id)
            => Ok(await getWithItemsUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/cancel")]
        public async Task<ActionResult<ServiceResult<string>>> Cancel(Guid id)
            => Ok(await cancelUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<ProposalDto>>>> GetByCustomer(Guid customerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId, onlyActive));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<ProposalDto>>>> GetByAffiliate(Guid affiliateId, [FromQuery] bool onlyActive = true)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId, onlyActive));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<ProposalDto>>>> GetByPartner(Guid partnerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive));

        [HttpPost("{id:guid}/generate-signature-link")]
        public async Task<ActionResult<ServiceResult<Uri>>> GenerateSignatureLink(Guid id)
            => Ok(await generateSignatureLinkUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/mark-as-signed")]
        public async Task<ActionResult<ServiceResult<string>>> MarkAsSigned(Guid id)
            => Ok(await markAsSignedUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/formalize")]
        public async Task<ActionResult<ServiceResult<string>>> Formalize(Guid id)
            => Ok(await formalizeUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/disburse")]
        public async Task<ActionResult<ServiceResult<string>>> Disburse(Guid id)
            => Ok(await markAsDisbursedUseCase.ExecuteAsync(id));

        // TODO Esse processo deverá ser disparado por um cron
        [HttpPost("{id:guid}/finalize")]
        public async Task<ActionResult<ServiceResult<string>>> Finalize(Guid id)
            => Ok(await markAsFinalizedUseCase.ExecuteAsync(id));

        [HttpPost("{id:guid}/generate-contract")]
        public async Task<ActionResult<ServiceResult<Uri>>> GenerateContract(Guid id)
            => Ok(await generateFinalContractUseCase.ExecuteAsync(id));
    }
}
