using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Repositories
{
    public sealed class SimulationRepository(IDbConnection connection) : ISimulationRepository
    {
        public async Task InsertAsync(Simulation simulation)
            => await connection.InsertAsync(simulation);

        public async Task<Simulation?> GetByIdAsync(Guid id)
            => await connection.GetAsync<Simulation>(id);

        public async Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId)
        {
            const string sql = """
            SELECT * FROM simulation
            WHERE customer_tax_id = @TaxId
            ORDER BY created_at DESC
            """;

            return await connection.QueryAsync<Simulation>(sql, new { TaxId = taxId });
        }
    }
}
