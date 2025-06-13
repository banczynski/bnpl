using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class CalculateInstallmentPenaltiesBatchUseCase(
        IInstallmentRepository installmentRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService
    )
    {
        public async Task<Result<List<InstallmentChargesReportItem>, string>> ExecuteAsync(DateTime? referenceDate = null)
        {
            var today = (referenceDate ?? DateTime.UtcNow).Date;
            var overdueInstallments = await installmentRepository.GetPendingDueInDaysAsync(-1);

            var results = new List<InstallmentChargesReportItem>();

            foreach (var installment in overdueInstallments.Where(i => i.DueDate < today && i.InvoiceId == null))
            {
                var config = await configService.GetEffectiveConfigAsync(installment.PartnerId, installment.AffiliateId);

                var charges = chargesCalculator.Calculate(new InstallmentChargesInput(
                    OriginalAmount: installment.Amount,
                    DueDate: installment.DueDate,
                    PaymentDate: today,
                    DailyInterestRate: config.InterestRate,
                    FixedChargesRate: config.ChargesRate
                ));

                results.Add(new InstallmentChargesReportItem(
                    InstallmentId: installment.Code,
                    PartnerId: installment.PartnerId,
                    AffiliateId: installment.AffiliateId,
                    CustomerTaxId: installment.CustomerTaxId,
                    Sequence: installment.Sequence,
                    DueDate: installment.DueDate,
                    DaysLate: charges.DaysLate,
                    OriginalAmount: installment.Amount,
                    FixedCharges: charges.FixedCharges,
                    InterestAmount: charges.InterestAmount,
                    TotalWithCharges: charges.TotalWithCharges
                ));
            }

            return Result<List<InstallmentChargesReportItem>, string>.Ok(results);
        }
    }
}
