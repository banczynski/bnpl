using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Application.Services.External;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GenerateInvoicePaymentLinkUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IChargesCalculatorService chargesService,
        IFinancialChargesConfigurationService configService,
        IPaymentLinkService paymentLinkService
    )
    {
        public async Task<ServiceResult<GeneratePaymentLinkResponse>> ExecuteAsync(Guid invoiceId)
        {
            var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new InvalidOperationException("Invoice not found.");

            var installments = await installmentRepository.GetByInvoiceIdAsync(invoiceId);
            if (!installments.Any())
                throw new InvalidOperationException("Invoice has no installments.");

            var config = await configService.GetEffectiveConfigAsync(invoice.PartnerId, invoice.AffiliateId);

            decimal totalWithCharges = 0m;
            var today = DateTime.UtcNow.Date;

            foreach (var i in installments)
            {
                var charges = chargesService.Calculate(new InstallmentChargesInput(
                    OriginalAmount: i.Amount,
                    DueDate: i.DueDate,
                    PaymentDate: today,
                    DailyInterestRate: config.InterestRate,
                    FixedChargesRate: config.ChargesRate
                ));

                totalWithCharges += charges.TotalWithCharges;
            }

            // TODO
            var paymentLink = await paymentLinkService.GenerateAsync(new PaymentLinkRequest(
                InvoiceId: invoice.Id,
                CustomerTaxId: invoice.CustomerTaxId,
                Amount: decimal.Round(totalWithCharges, 2),
                DueDate: invoice.DueDate
            ));

            return new ServiceResult<GeneratePaymentLinkResponse>(
                new GeneratePaymentLinkResponse(
                    InvoiceId: invoice.Id,
                    OriginalAmount: invoice.TotalAmount,
                    ChargesAmount: decimal.Round(totalWithCharges - invoice.TotalAmount, 2),
                    FinalAmount: decimal.Round(totalWithCharges, 2),
                    PaymentLink: paymentLink
                ),
                ["Payment link generated."]
            );
        }
    }
}
