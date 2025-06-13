using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed record MarkOverdueInvoicesRequestUseCase;

    public sealed class MarkOverdueInvoicesUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<MarkOverdueInvoicesRequestUseCase, Result<int, Error>>
    {
        public async Task<Result<int, Error>> ExecuteAsync(MarkOverdueInvoicesRequestUseCase request)
        {
            var today = DateTime.UtcNow.Date;
            var overdueInvoices = await invoiceRepository.GetOverduePendingAsync(today, unitOfWork.Transaction);
            int count = 0;
            var installments = new List<Domain.Entities.Installment>();

            foreach (var invoice in overdueInvoices)
            {
                invoice.MarkAsOverdue(today, userContext.GetRequiredUserId());
                count++;

                installments.AddRange(await installmentRepository.GetByInvoiceIdAsync(invoice.Code, unitOfWork.Transaction));
            }

            foreach (var i in installments)
            {
                if (i.Status != InstallmentStatus.Paid && i.IsActive)
                    i.MarkAsOverdue(today, userContext.GetRequiredUserId());
            }

            await invoiceRepository.UpdateManyAsync(overdueInvoices, unitOfWork.Transaction);
            await installmentRepository.UpdateManyAsync(installments, unitOfWork.Transaction);

            return Result<int, Error>.Ok(count);
        }
    }
}