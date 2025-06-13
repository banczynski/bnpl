using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class GetSimulationByIdUseCase(ISimulationRepository simulationRepository)
    {
        public async Task<Result<SimulationDto, Error>> ExecuteAsync(Guid id)
        {
            var simulation = await simulationRepository.GetByIdAsync(id);
            if (simulation is null)
                return Result<SimulationDto, Error>.Fail(DomainErrors.Simulation.NotFound);

            return Result<SimulationDto, Error>.Ok(simulation.ToDto());
        }
    }
}