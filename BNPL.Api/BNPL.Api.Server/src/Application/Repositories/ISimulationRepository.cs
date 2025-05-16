using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface ISimulationRepository
    {
        Task InsertAsync(Simulation simulation);
        Task<Simulation?> GetByIdAsync(Guid id);
        Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId);
    }
}
