using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GetInvoicesByCustomerIdUseCase(IInvoiceRepository invoiceRepository)
    {
        public async Task<Result<IEnumerable<InvoiceDto>, Error>> ExecuteAsync(Guid customerId)
        {
            var invoices = await invoiceRepository.GetActivesByCustomerIdAsync(customerId);
            return Result<IEnumerable<InvoiceDto>, Error>.Ok(invoices.Select(i => i.ToDto()));
        }
    }
}