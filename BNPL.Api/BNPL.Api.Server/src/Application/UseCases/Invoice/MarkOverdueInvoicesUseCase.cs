using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class MarkOverdueInvoicesUseCase(
        IInvoiceRepository invoiceRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<int>> ExecuteAsync()
        {
            var today = DateTime.UtcNow.Date;
            var overdueInvoices = await invoiceRepository.GetOverduePendingAsync(today);

            int count = 0;

            foreach (var invoice in overdueInvoices)
            {
                invoice.MarkAsOverdue(today, userContext.UserId);

                await invoiceRepository.UpdateAsync(invoice);
                count++;
            }

            return new ServiceResult<int>(count, [$"{count} overdue invoices marked."]);
        }
    }
}
