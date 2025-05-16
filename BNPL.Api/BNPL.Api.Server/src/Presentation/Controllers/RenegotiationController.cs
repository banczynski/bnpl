using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.UseCases.Renegotiation;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RenegotiationController(
        CreateRenegotiationUseCase createUseCase,
        ConfirmRenegotiationUseCase confirmUseCase,
        GetRenegotiationByIdUseCase getByIdUseCase,
        GetRenegotiationsByCustomerUseCase getByCustomerUseCase,
        GetRenegotiationsByPartnerUseCase getByPartnerUseCase,
        GetRenegotiationsByAffiliateUseCase getByAffiliateUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<Guid>>> Create([FromBody] CreateRenegotiationRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        [HttpPost("{id:guid}/confirm")]
        public async Task<ActionResult<ServiceResult<string>>> Confirm(Guid id)
            => Ok(await confirmUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<RenegotiationDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<RenegotiationDto>>>> GetByCustomer(Guid customerId)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<RenegotiationDto>>>> GetByPartner(Guid partnerId)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<RenegotiationDto>>>> GetByAffiliate(Guid affiliateId)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId));
    }
}
