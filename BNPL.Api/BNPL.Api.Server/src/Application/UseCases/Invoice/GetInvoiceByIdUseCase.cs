using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class GetInvoiceByIdUseCase(IInvoiceRepository repository)
    {
        public async Task<ServiceResult<InvoiceDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Invoice not found.");

            return new ServiceResult<InvoiceDto>(entity.ToDto());
        }
    }
}
