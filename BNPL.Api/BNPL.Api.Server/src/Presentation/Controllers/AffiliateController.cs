using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.UseCases.Affiliate;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
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
        [ProducesResponseType(typeof(Result<CreateAffiliateResponse, string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CreateAffiliateResponse, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreateAffiliateResponse, string>>> Create(
            [FromQuery] Guid partnerId,
            [FromBody] CreateAffiliateRequest request)
        {
            var result = await createUseCase.ExecuteAsync(partnerId, request);
            return result switch
            {
                Result<CreateAffiliateResponse, string>.Success s =>
                    CreatedAtAction(nameof(GetById), new { id = s.Value.Id }, result),
                _ => BadRequest(result)
            };
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<AffiliateDto, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<AffiliateDto, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<AffiliateDto, string>>> Update(
            Guid id,
            [FromBody] UpdateAffiliateRequest request)
        {
            var result = await updateUseCase.ExecuteAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<bool, string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<bool, string>>> Delete(Guid id)
        {
            var result = await inactivateUseCase.ExecuteAsync(id);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<AffiliateDto, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<AffiliateDto, string>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<AffiliateDto>, string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<AffiliateDto>, string>>> GetAllByPartner(
            [FromQuery] Guid partnerId,
            [FromQuery] bool onlyActive = true)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId, onlyActive);
            return Ok(result);
        }
    }
}