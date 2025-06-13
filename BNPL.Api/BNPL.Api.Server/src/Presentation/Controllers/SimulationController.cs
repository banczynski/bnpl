using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.UseCases.Simulation;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SimulationController(
        CreateSimulationUseCase createUseCase,
        GetSimulationByIdUseCase getByIdUseCase,
        GetSimulationsByCustomerTaxIdUseCase getByCustomerUseCase
    ) : ControllerBase
    {
        [HttpPost("{affiliateId:guid}")]
        public async Task<ActionResult<Result<SimulationWithInstallmentsResponse, string>>> Simulate(
            Guid affiliateId,
            [FromBody] CreateSimulationRequest request)
        {
            var result = await createUseCase.ExecuteAsync(affiliateId, request);
            return CreatedAtAction(nameof(GetById), new { id = result is Result<SimulationWithInstallmentsResponse, string>.Success success ? success.Value.Simulation.Id : default }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<SimulationDto, string[]>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{taxId}")]
        public async Task<ActionResult<Result<IEnumerable<SimulationDto>, string[]>>> GetByCustomer(string taxId)
            => Ok(await getByCustomerUseCase.ExecuteAsync(taxId));
    }
}