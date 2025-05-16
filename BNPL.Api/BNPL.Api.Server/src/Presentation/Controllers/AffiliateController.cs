using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.UseCases.Affiliate;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AffiliateController(
        CreateAffiliateUseCase createUseCase,
        UpdateAffiliateUseCase updateUseCase,
        InactivateAffiliateUseCase inactivateUseCase,
        GetAffiliateByIdUseCase getByIdUseCase,
        GetAffiliatesByPartnerUseCase getByPartnerUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<CreateAffiliateResponse>>> Create([FromBody] CreateAffiliateRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResult<AffiliateDto>>> Update(Guid id, [FromBody] UpdateAffiliateRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<AffiliateDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<AffiliateDto>>>> GetByPartner(Guid partnerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive));
    }
}
