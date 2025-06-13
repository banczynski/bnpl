using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class CalculateInstallmentPenaltiesBatchUseCase(
        IInstallmentRepository installmentRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService
    )
    {
        public async Task<Result<List<InstallmentChargesReportItem>, Error>> ExecuteAsync(DateTime? referenceDate = null)
        {
            var today = (referenceDate ?? DateTime.UtcNow).Date;
            var overdueInstallments = (await installmentRepository.GetPendingDueInDaysAsync(-1))
                .Where(i => i.DueDate < today && i.InvoiceId == null)
                .ToList();

            if (overdueInstallments.Count == 0)
                return Result<List<InstallmentChargesReportItem>, Error>.Ok([]);

            var results = new List<InstallmentChargesReportItem>();

            var configKeys = overdueInstallments
                .Select(i => (i.PartnerId, i.AffiliateId))
                .Distinct()
                .ToList();

            var configMap = new Dictionary<(Guid, Guid?), FinancialChargesConfiguration>();
            foreach (var (partnerId, affiliateId) in configKeys)
            {
                var config = await configService.GetEffectiveConfigAsync(partnerId, affiliateId);
                configMap[(partnerId, affiliateId)] = config;
            }

            foreach (var installment in overdueInstallments)
            {
                if (!configMap.TryGetValue((installment.PartnerId, installment.AffiliateId), out var config))
                {
                    continue;
                }

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

            return Result<List<InstallmentChargesReportItem>, Error>.Ok(results);
        }
    }
}