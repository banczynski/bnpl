using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.UseCases.Simulation;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SimulationController(
        IUseCase<CreateSimulationRequestUseCase, Result<SimulationWithInstallmentsResponse, Error>> createUseCase,
        GetSimulationByIdUseCase getByIdUseCase,
        GetSimulationsByCustomerTaxIdUseCase getByCustomerUseCase
    ) : ControllerBase
    {
        [HttpPost("{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<SimulationWithInstallmentsResponse, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<SimulationWithInstallmentsResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<SimulationWithInstallmentsResponse, Error>>> Simulate(
            Guid affiliateId,
            [FromBody] CreateSimulationRequest request)
        {
            var useCaseRequest = new CreateSimulationRequestUseCase(affiliateId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetSuccess(out var successValue))
                return CreatedAtAction(nameof(GetById), new { id = successValue.Simulation.Id }, result);

            return BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<SimulationDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<SimulationDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<SimulationDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("by-customer/{taxId}")]
        [ProducesResponseType(typeof(Result<IEnumerable<SimulationDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<SimulationDto>, Error>>> GetByCustomer(string taxId)
        {
            var result = await getByCustomerUseCase.ExecuteAsync(taxId);
            return Ok(result);
        }
    }
}