using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.UseCases.Partner;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
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
        public async Task<ActionResult<ServiceResult<CreatePartnerResponse>>> Create([FromBody] CreatePartnerRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResult<PartnerDto>>> Update(Guid id, [FromBody] UpdatePartnerRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<PartnerDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet]
        public async Task<ActionResult<ServiceResult<IEnumerable<PartnerDto>>>> GetAll([FromQuery] bool onlyActive = true)
            => Ok(await getAllUseCase.ExecuteAsync(onlyActive));
    }
}
