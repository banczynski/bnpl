using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class GetSimulationsByCustomerTaxIdUseCase(ISimulationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<Domain.Entities.Simulation>>> ExecuteAsync(string taxId)
        {
            var simulations = await repository.GetByCustomerTaxIdAsync(taxId);
            return new ServiceResult<IEnumerable<Domain.Entities.Simulation>>(simulations);
        }
    }
}
