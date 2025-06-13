using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class GetSimulationByIdUseCase(ISimulationRepository simulationRepository)
    {
        public async Task<Result<SimulationDto, string>> ExecuteAsync(Guid id)
        {
            var simulation = await simulationRepository.GetByIdAsync(id);
            if (simulation is null)
                return Result<SimulationDto, string>.Fail("Simulation not found.");

            return Result<SimulationDto, string>.Ok(simulation.ToDto());
        }
    }
}
