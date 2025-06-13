using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.UseCases.Partner;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PartnerController(
        IUseCase<CreatePartnerRequestUseCase, Result<CreatePartnerResponse, Error>> createUseCase,
        IUseCase<UpdatePartnerRequestUseCase, Result<PartnerDto, Error>> updateUseCase,
        IUseCase<InactivatePartnerRequestUseCase, Result<bool, Error>> inactivateUseCase,
        GetPartnerByIdUseCase getByIdUseCase,
        GetAllPartnersUseCase getAllUseCase
    ) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Result<CreatePartnerResponse, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<CreatePartnerResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreatePartnerResponse, Error>>> Create([FromBody] CreatePartnerRequest request)
        {
            var useCaseRequest = new CreatePartnerRequestUseCase(request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetSuccess(out var successValue))
                return CreatedAtAction(nameof(GetById), new { id = successValue.Id }, result);

            return BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<PartnerDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PartnerDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<PartnerDto, Error>>> Update(Guid id, [FromBody] UpdatePartnerRequest request)
        {
            var useCaseRequest = new UpdatePartnerRequestUseCase(id, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Inactivate(Guid id)
        {
            var useCaseRequest = new InactivatePartnerRequestUseCase(id);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<PartnerDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<PartnerDto>, Error>>> GetAll()
        {
            var result = await getAllUseCase.ExecuteAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<PartnerDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PartnerDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<PartnerDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}