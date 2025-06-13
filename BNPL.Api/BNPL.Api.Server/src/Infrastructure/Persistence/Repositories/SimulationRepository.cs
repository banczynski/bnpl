using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class SimulationRepository(IDbConnection connection) : GenericRepository<Simulation>(connection), ISimulationRepository
    {
        private const string GetByCustomerTaxIdSql = "SELECT * FROM simulation WHERE customer_tax_id = @TaxId ORDER BY created_at DESC";

        public async Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            return await Connection.QueryAsync<Simulation>(GetByCustomerTaxIdSql, new { TaxId = taxId }, transaction);
        }
    }
}