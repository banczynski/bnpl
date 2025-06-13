using BNPL.Api.Server.src.Application.Abstractions.Business;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed record GenerateInvoiceBatchRequestUseCase(int DaysAhead);

    public sealed class GenerateInvoiceBatchUseCase(
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        ICustomerBillingPreferencesRepository billingPreferencesRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<GenerateInvoiceBatchRequestUseCase, Result<List<InvoiceDto>, Error>>
    {
        public async Task<Result<List<InvoiceDto>, Error>> ExecuteAsync(GenerateInvoiceBatchRequestUseCase request)
        {
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();
            var pendingInstallments = (await installmentRepository.GetPendingDueInDaysAsync(request.DaysAhead, unitOfWork.Transaction))
                .Where(i => i.InvoiceId == null)
                .ToList();
            var configMap = new Dictionary<(Guid, Guid?), FinancialChargesConfiguration>();
            var chargeMap = new Dictionary<Guid, decimal>();
            foreach (var inst in pendingInstallments)
            {
                var key = (inst.PartnerId, inst.AffiliateId);
                if (!configMap.TryGetValue(key, out var config))
                {
                    config = await configService.GetEffectiveConfigAsync(inst.PartnerId, inst.AffiliateId);
                    configMap[key] = config;
                }
                var charges = chargesCalculator.Calculate(new InstallmentChargesInput(
                    OriginalAmount: inst.Amount,
                    DueDate: inst.DueDate,
                    PaymentDate: now.Date,
                    DailyInterestRate: config.InterestRate,
                    FixedChargesRate: config.ChargesRate
                ));
                chargeMap[inst.Code] = charges.TotalWithCharges;
            }
            var grouped = pendingInstallments
                .GroupBy(i => (i.CustomerId, i.AffiliateId))
                .ToDictionary(g => g.Key, g => g.ToList());
            var results = new List<InvoiceDto>();
            foreach (var ((customerId, affiliateId), installments) in grouped)
            {
                var preferences = await billingPreferencesRepository
                    .GetByCustomerIdAndAffiliateIdAsync(customerId, affiliateId, unitOfWork.Transaction);
                if (preferences is null)
                    continue;
                var invoiceDueDay = preferences.InvoiceDueDay;
                var isIndividual = !preferences.ConsolidatedInvoiceEnabled;
                DateTime GetInvoiceDueDate(Domain.Entities.Installment i)
                {
                    if (isIndividual)
                        return i.DueDate.Date;
                    var due = i.DueDate;
                    return new DateTime(due.Year, due.Month, 1)
                        .AddMonths(due.Day > invoiceDueDay ? 1 : 0)
                        .AddDays(invoiceDueDay - 1);
                }
                var groupedByDueDate = installments.GroupBy(GetInvoiceDueDate);
                foreach (var dueGroup in groupedByDueDate)
                {
                    var dueDate = dueGroup.Key;
                    var groupInstallments = dueGroup.ToList();
                    var existing = await invoiceRepository
                        .GetByCustomerAndDueDateAsync(customerId, dueDate, unitOfWork.Transaction);
                    Domain.Entities.Invoice invoice;
                    if (existing is not null)
                    {
                        invoice = existing;
                        invoice.TotalAmount += groupInstallments.Sum(i => chargeMap[i.Code]);
                        invoice.UpdatedAt = now;
                        invoice.UpdatedBy = userId;
                        await invoiceRepository.UpdateAsync(invoice, unitOfWork.Transaction);
                    }
                    else
                    {
                        invoice = new Domain.Entities.Invoice
                        {
                            Code = Guid.NewGuid(),
                            PartnerId = groupInstallments.First().PartnerId,
                            AffiliateId = affiliateId,
                            CustomerId = customerId,
                            CustomerTaxId = groupInstallments.First().CustomerTaxId,
                            TotalAmount = groupInstallments.Sum(i => chargeMap[i.Code]),
                            DueDate = dueDate,
                            Status = InvoiceStatus.Pending,
                            IsIndividual = isIndividual,
                            CreatedAt = now,
                            UpdatedAt = now,
                            CreatedBy = userId,
                            UpdatedBy = userId,
                            IsActive = true
                        };
                        await invoiceRepository.InsertAsync(invoice, unitOfWork.Transaction);
                    }
                    foreach (var installment in groupInstallments)
                    {
                        installment.InvoiceId = invoice.Code;
                        installment.UpdatedAt = now;
                        installment.UpdatedBy = userId;
                    }
                    await installmentRepository.UpdateManyAsync(groupInstallments, unitOfWork.Transaction);
                    results.Add(invoice.ToDto());
                }
            }
            return Result<List<InvoiceDto>, Error>.Ok(results);
        }
    }
}