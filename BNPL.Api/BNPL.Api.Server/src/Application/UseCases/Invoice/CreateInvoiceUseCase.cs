using BNPL.Api.Server.src.Application.Abstractions.Business;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed record CreateInvoiceRequestUseCase(Guid CustomerId, Guid AffiliateId);

    public sealed class CreateInvoiceUseCase(
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        ICustomerBillingPreferencesRepository billingPreferencesRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateInvoiceRequestUseCase, Result<IEnumerable<CreateInvoiceResponse>, Error>>
    {
        public async Task<Result<IEnumerable<CreateInvoiceResponse>, Error>> ExecuteAsync(CreateInvoiceRequestUseCase request)
        {
            var (customerId, affiliateId) = request;
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();
            var invoices = new List<CreateInvoiceResponse>();

            var preferences = await billingPreferencesRepository
                .GetByCustomerIdAndAffiliateIdAsync(customerId, affiliateId, unitOfWork.Transaction);
            if (preferences is null)
                return Result<IEnumerable<CreateInvoiceResponse>, Error>.Fail(DomainErrors.Billing.NotFound);

            var allInstallments = (await installmentRepository
                .GetAllOpenByCustomerIdAsync(customerId, unitOfWork.Transaction)).ToList();
            if (allInstallments.Count == 0)
                return Result<IEnumerable<CreateInvoiceResponse>, Error>.Fail(DomainErrors.Installment.NoOpenInstallments);

            var invoiceIds = allInstallments
                .Where(i => i.InvoiceId.HasValue)
                .Select(i => i.InvoiceId!.Value)
                .Distinct()
                .ToList();

            if (invoiceIds.Count != 0)
            {
                var oldInvoices = await invoiceRepository.GetByIdsAsync(invoiceIds, unitOfWork.Transaction);
                foreach (var inv in oldInvoices)
                {
                    inv.IsActive = false;
                    inv.UpdatedAt = now;
                    inv.UpdatedBy = userId;
                }
                await invoiceRepository.UpdateManyAsync(oldInvoices, unitOfWork.Transaction);
            }

            foreach (var inst in allInstallments)
            {
                inst.InvoiceId = null;
                inst.UpdatedAt = now;
                inst.UpdatedBy = userId;
            }

            var configMap = new Dictionary<(Guid, Guid?), FinancialChargesConfiguration>();
            var chargeMap = new Dictionary<Guid, decimal>();

            foreach (var inst in allInstallments)
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

            DateTime GetInvoiceDueDate(Domain.Entities.Installment i)
            {
                if (!preferences.ConsolidatedInvoiceEnabled)
                    return i.DueDate.Date;
                var day = preferences.InvoiceDueDay;
                return new DateTime(i.DueDate.Year, i.DueDate.Month, 1)
                    .AddMonths(i.DueDate.Day > day ? 1 : 0)
                    .AddDays(day - 1);
            }

            var grouped = allInstallments.GroupBy(GetInvoiceDueDate);

            foreach (var group in grouped)
            {
                var dueDate = group.Key;
                var groupList = group.ToList();
                var totalAmount = groupList.Sum(i => chargeMap[i.Code]);
                var invoice = new Domain.Entities.Invoice
                {
                    Code = Guid.NewGuid(),
                    PartnerId = groupList.First().PartnerId,
                    AffiliateId = groupList.First().AffiliateId,
                    CustomerId = groupList.First().CustomerId,
                    CustomerTaxId = groupList.First().CustomerTaxId,
                    TotalAmount = totalAmount,
                    DueDate = dueDate,
                    Status = InvoiceStatus.Pending,
                    IsIndividual = !preferences.ConsolidatedInvoiceEnabled,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    IsActive = true
                };
                await invoiceRepository.InsertAsync(invoice, unitOfWork.Transaction);
                foreach (var inst in groupList)
                {
                    inst.InvoiceId = invoice.Code;
                    inst.UpdatedAt = now;
                    inst.UpdatedBy = userId;
                }
                await installmentRepository.UpdateManyAsync(groupList, unitOfWork.Transaction);
                invoices.Add(new CreateInvoiceResponse(invoice.Code, invoice.DueDate, invoice.TotalAmount));
            }
            return Result<IEnumerable<CreateInvoiceResponse>, Error>.Ok(invoices);
        }
    }
}