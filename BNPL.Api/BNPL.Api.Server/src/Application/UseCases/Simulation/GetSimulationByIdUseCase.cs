using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class GetSimulationByIdUseCase(ISimulationRepository repository)
    {
        public async Task<ServiceResult<Domain.Entities.Simulation>> ExecuteAsync(Guid id)
        {
            var simulation = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Simulation not found.");

            return new ServiceResult<Domain.Entities.Simulation>(simulation);
        }
    }
}
