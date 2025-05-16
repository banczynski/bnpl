using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class CreateInvoiceUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CreateInvoiceResponse>> ExecuteAsync(CreateInvoiceRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var installments = (await installmentRepository.GetPendingByIdsAsync(request.InstallmentIds)).ToList();

            if (installments.Count != request.InstallmentIds.Count)
                throw new InvalidOperationException("One or more installments were not found or are invalid.");

            if (installments.Select(i => i.CustomerTaxId).Distinct().Count() > 1)
                throw new InvalidOperationException("Installments must belong to the same customer.");

            if (installments.Any(i => i.InvoiceId != null))
                throw new InvalidOperationException("Some installments are already invoiced.");

            var total = installments.Sum(i => i.Amount);
            if (total != request.TotalAmount)
                throw new InvalidOperationException("TotalAmount does not match the sum of installments.");

            var invoice = request.ToEntity(id, now, userContext.UserId);
            await invoiceRepository.InsertAsync(invoice);

            foreach (var i in installments)
            {
                i.InvoiceId = id;
                i.UpdatedAt = now;
                i.UpdatedBy = userContext.UserId;
                await installmentRepository.UpdateAsync(i);
            }

            return new ServiceResult<CreateInvoiceResponse>(
                new CreateInvoiceResponse(id),
                ["Invoice created successfully."]
            );
        }
    }
}
