namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }
    }
}
