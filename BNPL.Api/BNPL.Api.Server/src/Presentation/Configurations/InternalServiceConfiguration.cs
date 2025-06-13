using Amazon.S3;
using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Notification;
using BNPL.Api.Server.src.Application.Abstractions.Storage;
using BNPL.Api.Server.src.Infrastructure.Services;
using BNPL.Api.Server.src.Infrastructure.Services.Business;
using Core.Context;
using Core.Context.Interfaces;

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
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IInstallmentCalculator, InstallmentCalculator>();

            return services;
        }
    }
}
