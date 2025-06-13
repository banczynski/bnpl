using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class MarkOverdueInvoicesUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<int, string>> ExecuteAsync()
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var today = DateTime.UtcNow.Date;
                var overdueInvoices = await invoiceRepository.GetOverduePendingAsync(today, scope.Transaction);

                int count = 0;
                var installments = new List<Domain.Entities.Installment>();

                foreach (var invoice in overdueInvoices)
                {
                    invoice.MarkAsOverdue(today, userContext.GetRequiredUserId());
                    count++;

                    installments = [.. (await installmentRepository.GetByInvoiceIdAsync(invoice.Code, scope.Transaction))];
                    foreach (var i in installments)
                    {
                        if (i.Status != InstallmentStatus.Paid && i.IsActive)
                        {
                            i.MarkAsOverdue(today, userContext.GetRequiredUserId());
                        }
                    }
                }

                await invoiceRepository.UpdateManyAsync(overdueInvoices, scope.Transaction);
                await installmentRepository.UpdateManyAsync(installments, scope.Transaction);

                scope.Commit();

                return Result<int, string>.Ok(count);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
