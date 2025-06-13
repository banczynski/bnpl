using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Infrastructure.Persistence;
using System.Data;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork>(provider =>
            {
                var connectionString = configuration.GetConnectionString("PostgreSql")
                    ?? throw new InvalidOperationException("Missing connection string.");

                return new UnitOfWork(connectionString);
            });

            services.AddScoped<IDbConnection>(provider =>
                provider.GetRequiredService<IUnitOfWork>().Connection);

            return services;
        }
    }
}