using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GetInvoicesByCustomerIdUseCase(IInvoiceRepository invoiceRepository)
    {
        public async Task<Result<IEnumerable<InvoiceDto>, string>> ExecuteAsync(Guid customerId, bool onlyActive = true)
        {
            var invoices = await invoiceRepository.GetByCustomerIdAsync(customerId, onlyActive);
            return Result<IEnumerable<InvoiceDto>, string>.Ok(invoices.Select(i => i.ToDto()));
        }
    }
}
