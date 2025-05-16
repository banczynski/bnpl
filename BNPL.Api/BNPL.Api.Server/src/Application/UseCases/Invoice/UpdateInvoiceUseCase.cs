using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class UpdateInvoiceUseCase(
        IInvoiceRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<InvoiceDto>> ExecuteAsync(Guid id, UpdateInvoiceRequest request)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Invoice not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<InvoiceDto>(entity.ToDto(), ["Invoice updated successfully."]);
        }
    }
}
