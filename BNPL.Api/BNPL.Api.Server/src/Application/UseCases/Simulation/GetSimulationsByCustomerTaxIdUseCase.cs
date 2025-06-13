using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class GetSimulationsByCustomerTaxIdUseCase(ISimulationRepository simulationRepository)
    {
        public async Task<Result<IEnumerable<SimulationDto>, string>> ExecuteAsync(string taxId)
        {
            var simulations = await simulationRepository.GetByCustomerTaxIdAsync(taxId);
            return Result<IEnumerable<SimulationDto>, string>.Ok(simulations.ToDtoList());
        }
    }
}
