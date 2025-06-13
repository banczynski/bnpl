using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task UpdateManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null);
        Task InsertManyAsync(IEnumerable<Invoice> invoices, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetByIdsAsync(IEnumerable<Guid> codes, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetOverduePendingAsync(DateTime referenceDate, IDbTransaction? transaction = null);
        Task<IEnumerable<Invoice>> GetActivesByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
        Task<Invoice?> GetByCustomerAndDueDateAsync(Guid customerId, DateTime dueDate, IDbTransaction? transaction = null);
    }
}