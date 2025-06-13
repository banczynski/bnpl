using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GetInvoiceByIdUseCase(IInvoiceRepository invoiceRepository)
    {
        public async Task<Result<InvoiceDto, string>> ExecuteAsync(Guid id)
        {
            var entity = await invoiceRepository.GetByIdAsync(id);

            if (entity is null)
                return Result<InvoiceDto, string>.Fail("Invoice not found.");

            return Result<InvoiceDto, string>.Ok(entity.ToDto());
        }
    }
}
