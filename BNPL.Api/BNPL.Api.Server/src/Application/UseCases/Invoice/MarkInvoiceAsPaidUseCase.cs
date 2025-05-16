using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class MarkInvoiceAsPaidUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid invoiceId)
        {
            var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new InvalidOperationException("Invoice not found.");

            if (invoice.Status == InvoiceStatus.Paid)
                return new ServiceResult<string>("Invoice already marked as paid.");

            var now = DateTime.UtcNow;

            invoice.MarkAsPaid(now, userContext.UserId);

            await invoiceRepository.UpdateAsync(invoice);

            var installments = await installmentRepository.GetByInvoiceIdAsync(invoiceId);

            foreach (var i in installments)
            {
                i.MarkAsPaid(now, userContext.UserId);
                await installmentRepository.UpdateAsync(i);
            }

            return new ServiceResult<string>("Invoice and installments marked as paid.");
        }
    }
}
