using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Infrastructure.Services.External;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class ExternalServiceConfiguration
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<ICreditAnalysisService, FakeCreditAnalysisService>();
            services.AddScoped<IDocumentOcrService, FakeDocumentOcrService>();
            services.AddScoped<IFaceMatchService, FakeFaceMatchService>();
            services.AddScoped<IPaymentLinkService, FakePaymentLinkService>();
            services.AddScoped<IPdfContractService, FakePdfContractService>();
            services.AddScoped<ISignatureService, FakeSignatureService>();
            return services;
        }
    }
}
