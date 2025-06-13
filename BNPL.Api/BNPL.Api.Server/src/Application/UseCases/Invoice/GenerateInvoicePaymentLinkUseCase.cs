using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
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
        public async Task<Result<GeneratePaymentLinkResponse, string>> ExecuteAsync(Guid invoiceId)
        {
            var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice is null)
                return Result<GeneratePaymentLinkResponse, string>.Fail("Invoice not found.");

            var installments = await installmentRepository.GetByInvoiceIdAsync(invoiceId);
            if (!installments.Any())
                return Result<GeneratePaymentLinkResponse, string>.Fail("Invoice has no installments.");

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

            var paymentLink = await paymentLinkService.GenerateAsync(new PaymentLinkRequest(
                InvoiceId: invoice.Code,
                CustomerTaxId: invoice.CustomerTaxId,
                Amount: decimal.Round(totalWithCharges, 2),
                DueDate: invoice.DueDate
            ));

            var response = new GeneratePaymentLinkResponse(
                InvoiceId: invoice.Code,
                OriginalAmount: invoice.TotalAmount,
                ChargesAmount: decimal.Round(totalWithCharges - invoice.TotalAmount, 2),
                FinalAmount: decimal.Round(totalWithCharges, 2),
                PaymentLink: paymentLink
            );

            return Result<GeneratePaymentLinkResponse, string>.Ok(response);
        }
    }
}
