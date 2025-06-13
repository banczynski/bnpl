using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.DTOs.BillingPreferences;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
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
using Core.Models;
using Core.Persistence;
using Core.Persistence.Interfaces;

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

        private static IServiceCollection AddTransactionalUseCase<TUseCase, TRequest, TResponse>(this IServiceCollection services)
            where TUseCase : class, IUseCase<TRequest, TResponse>
            where TResponse : class, Core.Models.Interfaces.IResult
        {
            services.AddScoped<TUseCase>();
            services.AddScoped<IUseCase<TRequest, TResponse>>(provider =>
                new TransactionalUseCaseDecorator<TRequest, TResponse>(
                    provider.GetRequiredService<TUseCase>(),
                    provider.GetRequiredService<IUnitOfWork>()
                ));
            return services;
        }

        public static IServiceCollection AddProposalUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateProposalUseCase, CreateProposalRequestUseCase, Result<CreateProposalResponse, Error>>();
            services.AddTransactionalUseCase<CancelProposalUseCase, CancelProposalRequestUseCase, Result<bool, Error>>();
            services.AddTransactionalUseCase<CancelProposalsUseCase, CancelProposalsRequestUseCase, Result<int, Error>>();
            services.AddTransactionalUseCase<InactivateProposalUseCase, InactivateProposalRequestUseCase, Result<bool, Error>>();
            services.AddTransactionalUseCase<MarkProposalAsFinalizedUseCase, MarkProposalAsFinalizedRequestUseCase, Result<bool, Error>>();
            services.AddTransactionalUseCase<GenerateSignatureTokenUseCase, GenerateSignatureTokenRequestUseCase, Result<SignatureTokenResponse, Error>>();
            services.AddTransactionalUseCase<ConfirmSignatureUseCase, ConfirmSignatureRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetProposalByIdUseCase>();
            services.AddScoped<GetProposalsByAffiliateIdUseCase>();
            services.AddScoped<GetProposalsByPartnerIdUseCase>();
            services.AddScoped<GetProposalsByCustomerIdUseCase>();
            services.AddScoped<GenerateFinalContractUseCase>();
            services.AddScoped<GetProposalWithItemsUseCase>();
            return services;
        }

        public static IServiceCollection AddProposalItemUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateProposalItemUseCase, CreateProposalItemRequestUseCase, Result<ProposalItemDto, Error>>();
            services.AddTransactionalUseCase<CreateProposalItemsUseCase, CreateProposalItemsRequestUseCase, Result<IEnumerable<ProposalItemDto>, Error>>();
            services.AddTransactionalUseCase<ConfirmProposalItemReturnUseCase, ConfirmProposalItemReturnRequestUseCase, Result<bool, Error>>();
            services.AddTransactionalUseCase<MarkProposalItemAsReturnedUseCase, MarkProposalItemAsReturnedRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetProposalItemsByProposalIdUseCase>();
            return services;
        }

        public static IServiceCollection AddCustomerUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateCustomerUseCase, CreateCustomerRequestUseCase, Result<CreateCustomerResponse, Error>>();
            services.AddTransactionalUseCase<UpdateCustomerUseCase, UpdateCustomerRequestUseCase, Result<CustomerDto, Error>>();
            services.AddTransactionalUseCase<InactivateCustomerUseCase, InactivateCustomerRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetCustomerByIdUseCase>();
            services.AddScoped<GetCustomersByAffiliateUseCase>();
            services.AddScoped<GetCustomersByPartnerUseCase>();
            return services;
        }

        public static IServiceCollection AddKycUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateKycUseCase, CreateKycRequestUseCase, Result<KycDto, Error>>();
            services.AddTransactionalUseCase<UpdateKycUseCase, UpdateKycRequestUseCase, Result<KycDto, Error>>();
            services.AddTransactionalUseCase<AnalyzeCustomerDocumentUseCase, AnalyzeCustomerDocumentRequestUseCase, Result<OcrExtractionResult, Error>>();
            services.AddTransactionalUseCase<ValidateFaceMatchUseCase, ValidateFaceMatchRequestUseCase, Result<bool, Error>>();
            services.AddTransactionalUseCase<ValidateKycStatusUseCase, ValidateKycStatusRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetKycUseCase>();
            return services;
        }

        public static IServiceCollection AddAffiliateUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateAffiliateUseCase, CreateAffiliateRequestUseCase, Result<CreateAffiliateResponse, Error>>();
            services.AddTransactionalUseCase<UpdateAffiliateUseCase, UpdateAffiliateRequestUseCase, Result<AffiliateDto, Error>>();
            services.AddTransactionalUseCase<InactivateAffiliateUseCase, InactivateAffiliateRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetAffiliateByIdUseCase>();
            services.AddScoped<GetAffiliatesByPartnerUseCase>();
            return services;
        }

        public static IServiceCollection AddPartnerUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreatePartnerUseCase, CreatePartnerRequestUseCase, Result<CreatePartnerResponse, Error>>();
            services.AddTransactionalUseCase<UpdatePartnerUseCase, UpdatePartnerRequestUseCase, Result<PartnerDto, Error>>();
            services.AddTransactionalUseCase<InactivatePartnerUseCase, InactivatePartnerRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetPartnerByIdUseCase>();
            services.AddScoped<GetAllPartnersUseCase>();
            return services;
        }

        public static IServiceCollection AddSimulationUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateSimulationUseCase, CreateSimulationRequestUseCase, Result<SimulationWithInstallmentsResponse, Error>>();

            services.AddScoped<GetSimulationByIdUseCase>();
            services.AddScoped<GetSimulationsByCustomerTaxIdUseCase>();
            return services;
        }

        public static IServiceCollection AddInvoiceUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateInvoiceUseCase, CreateInvoiceRequestUseCase, Result<IEnumerable<CreateInvoiceResponse>, Error>>();
            services.AddTransactionalUseCase<GenerateInvoiceBatchUseCase, GenerateInvoiceBatchRequestUseCase, Result<List<InvoiceDto>, Error>>();
            services.AddTransactionalUseCase<MarkOverdueInvoicesUseCase, MarkOverdueInvoicesRequestUseCase, Result<int, Error>>();

            services.AddScoped<GenerateInvoicePaymentLinkUseCase>();
            services.AddScoped<GetInvoiceByIdUseCase>();
            services.AddScoped<GetInvoicesByCustomerIdUseCase>();
            return services;
        }

        public static IServiceCollection AddInstallmentUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<GenerateInstallmentsUseCase, GenerateInstallmentsRequestUseCase, Result<IEnumerable<InstallmentDto>, Error>>();

            services.AddScoped<CalculateInstallmentChargesUseCase>();
            services.AddScoped<CalculateInstallmentPenaltiesBatchUseCase>();
            services.AddScoped<GetInstallmentsByCustomerUseCase>();
            services.AddScoped<GetInstallmentsByProposalUseCase>();
            return services;
        }

        public static IServiceCollection AddPaymentUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<PayInvoiceUseCase, PayInvoiceRequestUseCase, Result<bool, Error>>();
            return services;
        }

        public static IServiceCollection AddCreditAnalysisUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<RunCreditAnalysisUseCase, RunCreditAnalysisRequestUseCase, Result<CreditAnalysisResult, Error>>();
            services.AddTransactionalUseCase<UpdateCreditAnalysisConfigUseCase, UpdateCreditAnalysisConfigRequestUseCase, Result<CreditAnalysisConfigDto, Error>>();
            services.AddTransactionalUseCase<InactivateCreditAnalysisConfigUseCase, InactivateCreditAnalysisConfigRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetCreditAnalysisConfigByAffiliateUseCase>();
            services.AddScoped<GetCreditAnalysisConfigByPartnerUseCase>();
            services.AddScoped<EvaluateCustomerCreditUseCase>();
            return services;
        }

        public static IServiceCollection AddFinancialChargesUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<CreateFinancialChargesConfigUseCase, CreateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>>();
            services.AddTransactionalUseCase<UpdateFinancialChargesConfigUseCase, UpdateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>>();
            services.AddTransactionalUseCase<InactivateFinancialChargesConfigUseCase, InactivateFinancialChargesConfigRequestUseCase, Result<bool, Error>>();

            services.AddScoped<GetFinancialChargesByAffiliateUseCase>();
            services.AddScoped<GetFinancialChargesByPartnerUseCase>();
            return services;
        }

        public static IServiceCollection AddCreditLimitUseCases(this IServiceCollection services) => services
            .AddScoped<GetCustomerCreditLimitUseCase>()
            .AddScoped<AdjustCustomerCreditLimitUseCase>()
            .AddScoped<UpsertCustomerCreditLimitUseCase>();

        public static IServiceCollection AddBillingPreferenceUseCases(this IServiceCollection services)
        {
            services.AddTransactionalUseCase<UpdateCustomerBillingPreferencesUseCase, UpdateCustomerBillingPreferencesRequestUseCase, Result<CustomerBillingPreferencesDto, Error>>();
            return services;
        }
    }
}