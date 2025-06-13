using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Dapper;
using System.Data;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public sealed class SimulationRepository(IDbConnection connection) : ISimulationRepository
    {
        public async Task InsertAsync(Simulation simulation, IDbTransaction? transaction = null)
            => await connection.InsertAsync(simulation, transaction);

        public async Task<Simulation?> GetByIdAsync(Guid id, IDbTransaction? transaction = null)
        {
            const string sql = "SELECT * FROM simulation WHERE code = @Id LIMIT 1";
            return await connection.QuerySingleOrDefaultAsync<Simulation>(sql, new { Id = id }, transaction);
        }

        public async Task<IEnumerable<Simulation>> GetByCustomerTaxIdAsync(string taxId, IDbTransaction? transaction = null)
        {
            const string sql = """
            SELECT * FROM simulation
            WHERE customer_tax_id = @TaxId
            ORDER BY created_at DESC
            """;

            return await connection.QueryAsync<Simulation>(sql, new { TaxId = taxId }, transaction);
        }
    }
}
