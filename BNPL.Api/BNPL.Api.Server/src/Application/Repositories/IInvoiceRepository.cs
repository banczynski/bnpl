using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IInvoiceRepository
    {
        Task InsertAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt);
        Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate);
        Task<Invoice?> GetByIdAsync(Guid id);
        Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true);
    }
}
