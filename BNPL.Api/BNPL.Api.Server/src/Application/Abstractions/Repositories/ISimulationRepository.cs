using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ISimulationRepository : IGenericRepository<Simulation>
    {
        Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId, IDbTransaction? transaction = null);
    }
}