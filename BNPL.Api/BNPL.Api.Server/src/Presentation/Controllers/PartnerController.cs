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
        CreatePartnerUseCase createUseCase,
        UpdatePartnerUseCase updateUseCase,
        InactivatePartnerUseCase inactivateUseCase,
        GetPartnerByIdUseCase getByIdUseCase,
        GetAllPartnersUseCase getAllUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result<CreatePartnerResponse, string[]>>> Create([FromBody] CreatePartnerRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);

            if (result is Result<CreatePartnerResponse, string[]>.Success success)
                return CreatedAtAction(nameof(GetById), new { id = success.Value.Id }, result);

            return BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<PartnerDto, string>>> Update(Guid id, [FromBody] UpdatePartnerRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Result<bool, string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<PartnerDto, string>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<PartnerDto>, string>>> GetAll([FromQuery] bool onlyActive = true)
            => Ok(await getAllUseCase.ExecuteAsync(onlyActive));
    }
}
