using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.UseCases.Simulation;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SimulationController(
        CreateSimulationUseCase createUseCase,
        GetSimulationByIdUseCase getByIdUseCase,
        GetSimulationsByCustomerTaxIdUseCase getByCustomerUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<SimulationResponse>>> Simulate([FromBody] CreateSimulationRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<Simulation>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{taxId}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<Simulation>>>> GetByCustomer(string taxId)
            => Ok(await getByCustomerUseCase.ExecuteAsync(taxId));
    }
}
