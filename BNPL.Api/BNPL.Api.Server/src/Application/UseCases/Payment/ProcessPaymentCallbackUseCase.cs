using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Payment;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Payment
{
    public sealed class ProcessPaymentCallbackUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IProposalRepository proposalRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(PaymentCallbackRequest request)
        {
            var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId)
                ?? throw new InvalidOperationException("Invoice not found.");

            var installments = await installmentRepository.GetByInvoiceIdAsync(request.InvoiceId);
            if (!installments.Any())
                throw new InvalidOperationException("Invoice has no installments.");

            var config = await configService.GetEffectiveConfigAsync(invoice.PartnerId, invoice.AffiliateId);

            var paymentDate = request.PaymentDate?.Date ?? DateTime.UtcNow.Date;
            decimal totalWithCharges = 0m;

            foreach (var i in installments)
            {
                var charges = chargesCalculator.Calculate(new InstallmentChargesInput(
                    OriginalAmount: i.Amount,
                    DueDate: i.DueDate,
                    PaymentDate: paymentDate,
                    DailyInterestRate: config.InterestRate,
                    FixedChargesRate: config.ChargesRate
                ));

                totalWithCharges += charges.TotalWithCharges;
            }

            if (request.PaidAmount < totalWithCharges)
                throw new InvalidOperationException("Paid amount is less than total due with penalties.");

            var now = DateTime.UtcNow;

            invoice.MarkAsPaid(now, userContext.UserId);
            await invoiceRepository.UpdateAsync(invoice);

            foreach (var i in installments)
            {
                i.MarkAsPaid(now, userContext.UserId);
                await installmentRepository.UpdateAsync(i);
            }

            var firstInstallmentWithProposal = installments.FirstOrDefault(i => i.ProposalId.HasValue);
            if (firstInstallmentWithProposal?.ProposalId is Guid proposalId)
            {
                var allInstallments = await installmentRepository.GetByProposalIdAsync(proposalId);
                if (allInstallments.All(i => i.Status == InstallmentStatus.Paid))
                {
                    var proposal = await proposalRepository.GetByIdAsync(proposalId)
                        ?? throw new InvalidOperationException("Proposal not found.");

                    proposal.MarkAsFinalized(now, userContext.UserId);
                    await proposalRepository.UpdateAsync(proposal);

                    await adjustCreditLimitUseCase.ExecuteAsync(
                        invoice.CustomerTaxId,
                        invoice.TotalAmount,
                        increase: true
                    );
                }
            }

            return new ServiceResult<string>("Payment processed successfully.");
        }
    }
}
