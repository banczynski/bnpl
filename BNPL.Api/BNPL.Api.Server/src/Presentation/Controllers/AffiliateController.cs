using Core.Persistence.Interfaces;
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
        IUseCase<CreateAffiliateRequestUseCase, Result<CreateAffiliateResponse, Error>> createUseCase,
        IUseCase<UpdateAffiliateRequestUseCase, Result<AffiliateDto, Error>> updateUseCase,
        IUseCase<InactivateAffiliateRequestUseCase, Result<bool, Error>> inactivateUseCase,
        GetAffiliateByIdUseCase getByIdUseCase,
        GetAffiliatesByPartnerUseCase getByPartnerUseCase
    ) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Result<CreateAffiliateResponse, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CreateAffiliateResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreateAffiliateResponse, Error>>> Create(
            [FromQuery] Guid partnerId,
            [FromBody] CreateAffiliateRequest request)
        {
            var useCaseRequest = new CreateAffiliateRequestUseCase(partnerId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetSuccess(out var successValue))
                return CreatedAtAction(nameof(GetById), new { id = successValue.Id }, result);

            return BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<AffiliateDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<AffiliateDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<AffiliateDto, Error>>> Update(
            Guid id,
            [FromBody] UpdateAffiliateRequest request)
        {
            var useCaseRequest = new UpdateAffiliateRequestUseCase(id, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Delete(Guid id)
        {
            var useCaseRequest = new InactivateAffiliateRequestUseCase(id);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<AffiliateDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<AffiliateDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<AffiliateDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<AffiliateDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<AffiliateDto>, Error>>> GetAllByPartner(
            [FromQuery] Guid partnerId)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId);

            return Ok(result);
        }
    }
}