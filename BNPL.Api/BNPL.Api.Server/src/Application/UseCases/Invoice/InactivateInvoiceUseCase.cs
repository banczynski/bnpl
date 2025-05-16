using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Invoice
{
    public sealed class InactivateInvoiceUseCase(
        IInvoiceRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid id)
        {
            var now = DateTime.UtcNow;

            await repository.InactivateAsync(id, userContext.UserId, now);

            return new ServiceResult<string>("Invoice inactivated successfully.");
        }
    }
}
