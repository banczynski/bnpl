using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IInvoiceRepository
    {
        Task InsertAsync(Invoice invoice, IDbTransaction? transaction = null);
        Task UpdateAsync(Invoice invoice, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null);
        Task UpdateManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null);
        Task InsertManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate, IDbTransaction? transaction = null);
        Task<Invoice?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<Invoice?> GetByCustomerAndDueDateAsync(Guid customerId, DateTime dueDate, IDbTransaction? transaction = null);
    }
}
