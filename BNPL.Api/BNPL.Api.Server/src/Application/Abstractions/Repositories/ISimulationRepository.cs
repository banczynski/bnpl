using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ISimulationRepository
    {
        Task InsertAsync(Simulation simulation, IDbTransaction? transaction = null);
        Task<Simulation?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId, IDbTransaction? transaction = null);
    }
}
