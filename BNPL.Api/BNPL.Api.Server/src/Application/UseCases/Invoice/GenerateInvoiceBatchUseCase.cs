using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GenerateInvoiceBatchUseCase(
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<List<InvoiceDto>>> ExecuteAsync(int daysAhead = 0)
        {
            var now = DateTime.UtcNow;
            var installments = await installmentRepository.GetPendingDueInDaysAsync(daysAhead);

            var grouped = installments
                .Where(i => i.InvoiceId == null)
                .GroupBy(i => i.CustomerTaxId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var results = new List<InvoiceDto>();

            foreach (var (taxId, customerInstallments) in grouped)
            {
                var id = Guid.NewGuid();
                var total = customerInstallments.Sum(i => i.Amount);
                var dueDate = customerInstallments.Min(i => i.DueDate);

                var invoice = new Domain.Entities.Invoice
                {
                    Id = id,
                    CustomerTaxId = taxId,
                    DueDate = dueDate,
                    TotalAmount = total,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = userContext.UserId,
                    UpdatedBy = userContext.UserId,
                    IsActive = true
                };

                await invoiceRepository.InsertAsync(invoice);

                foreach (var i in customerInstallments)
                {
                    i.InvoiceId = id;
                    i.UpdatedAt = now;
                    i.UpdatedBy = userContext.UserId;
                    await installmentRepository.UpdateAsync(i);
                }

                results.Add(invoice.ToDto());
            }

            return new ServiceResult<List<InvoiceDto>>(results, ["Batch of invoices generated."]);
        }
    }
}
