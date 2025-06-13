using BNPL.Api.Server.src.Application.UseCases.Affiliate;
using BNPL.Api.Server.src.Application.UseCases.BillingPreferences;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Application.UseCases.Customer;
using BNPL.Api.Server.src.Application.UseCases.FinancialCharges;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using BNPL.Api.Server.src.Application.UseCases.Invoice;
using BNPL.Api.Server.src.Application.UseCases.Kyc;
using BNPL.Api.Server.src.Application.UseCases.Partner;
using BNPL.Api.Server.src.Application.UseCases.Payment;
using BNPL.Api.Server.src.Application.UseCases.Proposal;
using BNPL.Api.Server.src.Application.UseCases.ProposalItem;
using BNPL.Api.Server.src.Application.UseCases.Signature;
using BNPL.Api.Server.src.Application.UseCases.Simulation;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddUseCasesConfiguration(this IServiceCollection services)
        {
            return services
                .AddProposalUseCases()
                .AddProposalItemUseCases()
                .AddCustomerUseCases()
                .AddKycUseCases()
                .AddAffiliateUseCases()
                .AddPartnerUseCases()
                .AddSimulationUseCases()
                .AddInvoiceUseCases()
                .AddInstallmentUseCases()
                .AddPaymentUseCases()
                .AddCreditAnalysisUseCases()
                .AddFinancialChargesUseCases()
                .AddCreditLimitUseCases()
                .AddBillingPreferenceUseCases();
        }

        public static IServiceCollection AddProposalUseCases(this IServiceCollection services) => services
            .AddScoped<CreateProposalUseCase>()
            .AddScoped<GetProposalByIdUseCase>()
            .AddScoped<GetProposalsByAffiliateIdUseCase>()
            .AddScoped<GetProposalsByPartnerIdUseCase>()
            .AddScoped<GetProposalsByCustomerIdUseCase>()
            .AddScoped<CancelProposalUseCase>()
            .AddScoped<MarkProposalAsFinalizedUseCase>()
            .AddScoped<GenerateFinalContractUseCase>()
            .AddScoped<GenerateSignatureTokenUseCase>()
            .AddScoped<ConfirmSignatureUseCase>()
            .AddScoped<GetProposalWithItemsUseCase>()
            .AddScoped<InactivateProposalUseCase>()
            .AddScoped<CancelProposalsUseCase>();

        public static IServiceCollection AddProposalItemUseCases(this IServiceCollection services) => services
            .AddScoped<CreateProposalItemUseCase>()
            .AddScoped<CreateProposalItemsUseCase>()
            .AddScoped<GetProposalItemsByProposalIdUseCase>();

        public static IServiceCollection AddCustomerUseCases(this IServiceCollection services) => services
            .AddScoped<CreateCustomerUseCase>()
            .AddScoped<GetCustomerByIdUseCase>()
            .AddScoped<GetCustomersByAffiliateUseCase>()
            .AddScoped<GetCustomersByPartnerUseCase>()
            .AddScoped<UpdateCustomerUseCase>()
            .AddScoped<InactivateCustomerUseCase>();

        public static IServiceCollection AddKycUseCases(this IServiceCollection services) => services
            .AddScoped<CreateKycUseCase>()
            .AddScoped<GetKycUseCase>()
            .AddScoped<UpdateKycUseCase>()
            .AddScoped<AnalyzeCustomerDocumentUseCase>()
            .AddScoped<ValidateFaceMatchUseCase>()
            .AddScoped<ValidateKycStatusUseCase>();

        public static IServiceCollection AddAffiliateUseCases(this IServiceCollection services) => services
            .AddScoped<CreateAffiliateUseCase>()
            .AddScoped<GetAffiliateByIdUseCase>()
            .AddScoped<GetAffiliatesByPartnerUseCase>()
            .AddScoped<UpdateAffiliateUseCase>()
            .AddScoped<InactivateAffiliateUseCase>();

        public static IServiceCollection AddPartnerUseCases(this IServiceCollection services) => services
            .AddScoped<CreatePartnerUseCase>()
            .AddScoped<GetPartnerByIdUseCase>()
            .AddScoped<GetAllPartnersUseCase>()
            .AddScoped<UpdatePartnerUseCase>()
            .AddScoped<InactivatePartnerUseCase>();

        public static IServiceCollection AddSimulationUseCases(this IServiceCollection services) => services
            .AddScoped<CreateSimulationUseCase>()
            .AddScoped<GetSimulationByIdUseCase>()
            .AddScoped<GetSimulationsByCustomerTaxIdUseCase>();

        public static IServiceCollection AddInvoiceUseCases(this IServiceCollection services) => services
            .AddScoped<CreateInvoiceUseCase>()
            .AddScoped<GenerateInvoiceBatchUseCase>()
            .AddScoped<GenerateInvoicePaymentLinkUseCase>()
            .AddScoped<GetInvoiceByIdUseCase>()
            .AddScoped<GetInvoicesByCustomerIdUseCase>()
            .AddScoped<MarkOverdueInvoicesUseCase>();

        public static IServiceCollection AddInstallmentUseCases(this IServiceCollection services) => services
            .AddScoped<CalculateInstallmentChargesUseCase>()
            .AddScoped<CalculateInstallmentPenaltiesBatchUseCase>()
            .AddScoped<GetInstallmentsByCustomerUseCase>()
            .AddScoped<GetInstallmentsByProposalUseCase>();

        public static IServiceCollection AddPaymentUseCases(this IServiceCollection services) => services
            .AddScoped<PayInvoiceUseCase>();

        public static IServiceCollection AddCreditAnalysisUseCases(this IServiceCollection services) => services
            .AddScoped<GetCreditAnalysisConfigByAffiliateUseCase>()
            .AddScoped<GetCreditAnalysisConfigByPartnerUseCase>()
            .AddScoped<UpdateCreditAnalysisConfigUseCase>()
            .AddScoped<InactivateCreditAnalysisConfigUseCase>()
            .AddScoped<EvaluateCustomerCreditUseCase>()
            .AddScoped<RunCreditAnalysisUseCase>();

        public static IServiceCollection AddFinancialChargesUseCases(this IServiceCollection services) => services
            .AddScoped<CreateFinancialChargesConfigUseCase>()
            .AddScoped<GetFinancialChargesByAffiliateUseCase>()
            .AddScoped<GetFinancialChargesByPartnerUseCase>()
            .AddScoped<UpdateFinancialChargesConfigUseCase>()
            .AddScoped<InactivateFinancialChargesConfigUseCase>();

        public static IServiceCollection AddCreditLimitUseCases(this IServiceCollection services) => services
            .AddScoped<GetCustomerCreditLimitUseCase>()
            .AddScoped<AdjustCustomerCreditLimitUseCase>()
            .AddScoped<UpsertCustomerCreditLimitUseCase>();

        public static IServiceCollection AddBillingPreferenceUseCases(this IServiceCollection services) => services
            .AddScoped<UpdateCustomerBillingPreferencesUseCase>();
    }
}
