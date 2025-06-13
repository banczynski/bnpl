using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Infrastructure.Persistence.Repositories;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAffiliateRepository, AffiliateRepository>();
            services.AddScoped<ICreditAnalysisConfigurationRepository, CreditAnalysisConfigurationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerCreditLimitRepository, CustomerCreditLimitRepository>();
            services.AddScoped<IFinancialChargesConfigurationRepository, FinancialChargesConfigurationRepository>();
            services.AddScoped<IInstallmentRepository, InstallmentRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IKycRepository, KycRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IProposalItemRepository, ProposalItemRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped<IProposalSignatureRepository, ProposalSignatureRepository>();
            services.AddScoped<ISimulationRepository, SimulationRepository>();
            services.AddScoped<ICustomerBillingPreferencesRepository, CustomerBillingPreferencesRepository>();
            return services;
        }
    }
}
