using Amazon.S3;
using BNPL.Api.Server.src.Application.Context;
using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Infrastructure.Services;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class InternalServiceConfiguration
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IChargesCalculatorService, ChargesCalculatorService>();
            services.AddScoped<ICreditAnalysisConfigurationService, CreditAnalysisConfigurationService>();
            services.AddScoped<IFinancialChargesConfigurationService, FinancialChargesConfigurationService>();
            services.AddScoped<IS3StorageService, S3StorageService>();
            return services;
        }
    }
}
