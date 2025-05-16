using Npgsql;
using System.Data;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(configuration.GetConnectionString("PostgreSql")));

            return services;
        }
    }
}
