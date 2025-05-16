using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GetInvoicesByCustomerIdUseCase(IInvoiceRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<InvoiceDto>>> ExecuteAsync(Guid customerId, bool onlyActive = true)
        {
            var invoices = await repository.GetByCustomerIdAsync(customerId, onlyActive);
            return new ServiceResult<IEnumerable<InvoiceDto>>(invoices.Select(i => i.ToDto()));
        }
    }
}
