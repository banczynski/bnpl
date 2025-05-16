using BNPL.Api.Server.src.Application.UseCases.Proposal;
using BNPL.Api.Server.src.Application.UseCases.ProposalItem;
using BNPL.Api.Server.src.Application.UseCases.Customer;
using BNPL.Api.Server.src.Application.UseCases.Kyc;
using BNPL.Api.Server.src.Application.UseCases.Affiliate;
using BNPL.Api.Server.src.Application.UseCases.Partner;
using BNPL.Api.Server.src.Application.UseCases.Simulation;
using BNPL.Api.Server.src.Application.UseCases.Signature;
using BNPL.Api.Server.src.Application.UseCases.Invoice;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using BNPL.Api.Server.src.Application.UseCases.Renegotiation;
using BNPL.Api.Server.src.Application.UseCases.Payment;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using BNPL.Api.Server.src.Application.UseCases.FinancialCharges;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            // Proposal
            services.AddScoped<CreateProposalUseCase>();
            services.AddScoped<GetProposalByIdUseCase>();
            services.AddScoped<GetProposalsByAffiliateIdUseCase>();
            services.AddScoped<GetProposalsByPartnerIdUseCase>();
            services.AddScoped<GetProposalsByCustomerIdUseCase>();
            services.AddScoped<UpdateProposalUseCase>();
            services.AddScoped<CancelProposalUseCase>();
            services.AddScoped<MarkProposalAsSignedUseCase>();
            services.AddScoped<FormalizeProposalUseCase>();
            services.AddScoped<MarkProposalAsDisbursedUseCase>();
            services.AddScoped<MarkProposalAsFinalizedUseCase>();
            services.AddScoped<GenerateFinalContractUseCase>();
            services.AddScoped<GenerateSignatureLinkUseCase>();
            services.AddScoped<ProcessSignatureCallbackUseCase>();
            services.AddScoped<GetProposalWithItemsUseCase>();
            services.AddScoped<InactivateProposalUseCase>();

            // ProposalItem
            services.AddScoped<CreateProposalItemUseCase>();
            services.AddScoped<CreateProposalItemsUseCase>();
            services.AddScoped<GetProposalItemsByProposalIdUseCase>();
            services.AddScoped<MarkProposalItemAsReturnedUseCase>();

            // Customer
            services.AddScoped<CreateCustomerUseCase>();
            services.AddScoped<GetCustomerByIdUseCase>();
            services.AddScoped<GetCustomersByAffiliateUseCase>();
            services.AddScoped<GetCustomersByPartnerUseCase>();
            services.AddScoped<UpdateCustomerUseCase>();
            services.AddScoped<InactivateCustomerUseCase>();

            // KYC
            services.AddScoped<CreateKycUseCase>();
            services.AddScoped<GetKycUseCase>();
            services.AddScoped<UpdateKycUseCase>();
            services.AddScoped<AnalyzeCustomerDocumentUseCase>();
            services.AddScoped<ValidateFaceMatchUseCase>();
            services.AddScoped<ValidateKycStatusUseCase>();

            // Affiliate
            services.AddScoped<CreateAffiliateUseCase>();
            services.AddScoped<GetAffiliateByIdUseCase>();
            services.AddScoped<GetAffiliatesByPartnerUseCase>();
            services.AddScoped<UpdateAffiliateUseCase>();
            services.AddScoped<InactivateAffiliateUseCase>();

            // Partner
            services.AddScoped<CreatePartnerUseCase>();
            services.AddScoped<GetPartnerByIdUseCase>();
            services.AddScoped<GetAllPartnersUseCase>();
            services.AddScoped<UpdatePartnerUseCase>();
            services.AddScoped<InactivatePartnerUseCase>();

            // Simulation
            services.AddScoped<CreateSimulationUseCase>();
            services.AddScoped<GetSimulationByIdUseCase>();
            services.AddScoped<GetSimulationsByCustomerTaxIdUseCase>();

            // Invoice
            services.AddScoped<CreateInvoiceUseCase>();
            services.AddScoped<GenerateInvoiceBatchUseCase>();
            services.AddScoped<GenerateInvoicePaymentLinkUseCase>();
            services.AddScoped<GetInvoiceByIdUseCase>();
            services.AddScoped<GetInvoicesByCustomerIdUseCase>();
            services.AddScoped<UpdateInvoiceUseCase>();
            services.AddScoped<MarkInvoiceAsPaidUseCase>();
            services.AddScoped<MarkOverdueInvoicesUseCase>();
            services.AddScoped<InactivateInvoiceUseCase>();

            // Installment
            services.AddScoped<CalculateInstallmentChargesUseCase>();
            services.AddScoped<CalculateInstallmentPenaltiesBatchUseCase>();
            services.AddScoped<GetInstallmentsByCustomerUseCase>();
            services.AddScoped<GetInstallmentsByProposalUseCase>();

            // Renegotiation
            services.AddScoped<CreateRenegotiationUseCase>();
            services.AddScoped<CreateRenegotiationFromReturnedItemUseCase>();
            services.AddScoped<ConfirmRenegotiationUseCase>();
            services.AddScoped<GetRenegotiationByIdUseCase>();
            services.AddScoped<GetRenegotiationsByAffiliateUseCase>();
            services.AddScoped<GetRenegotiationsByCustomerUseCase>();
            services.AddScoped<GetRenegotiationsByPartnerUseCase>();

            // Payment
            services.AddScoped<ProcessPaymentCallbackUseCase>();

            // CreditAnalysis
            services.AddScoped<CreateCreditAnalysisConfigUseCase>();
            services.AddScoped<GetCreditAnalysisConfigByAffiliateUseCase>();
            services.AddScoped<GetCreditAnalysisConfigByPartnerUseCase>();
            services.AddScoped<UpdateCreditAnalysisConfigUseCase>();
            services.AddScoped<InactivateCreditAnalysisConfigUseCase>();

            // FinancialCharges
            services.AddScoped<CreateFinancialChargesConfigUseCase>();
            services.AddScoped<GetFinancialChargesByAffiliateUseCase>();
            services.AddScoped<GetFinancialChargesByPartnerUseCase>();
            services.AddScoped<UpdateFinancialChargesConfigUseCase>();
            services.AddScoped<InactivateFinancialChargesConfigUseCase>();

            // CreditLimit
            services.AddScoped<GetCustomerCreditLimitUseCase>();
            services.AddScoped<AdjustCustomerCreditLimitUseCase>();
            services.AddScoped<UpsertCustomerCreditLimitUseCase>();

            return services;
        }
    }
}