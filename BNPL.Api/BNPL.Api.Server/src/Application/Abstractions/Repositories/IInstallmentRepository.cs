using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface IInstallmentRepository
    {
        Task UpdateAsync(Installment installment, IDbTransaction? transaction = null);
        Task InsertManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null);
        Task UpdateManyAsync(IEnumerable<Installment> installments, IDbTransaction? transaction = null);
        Task<Installment?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetByProposalIdAsync(Guid proposalId, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetByInvoiceIdAsync(Guid invoiceId, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetPendingByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetPendingDueInDaysAsync(int days, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetPendingByIdsAsync(IEnumerable<Guid> ids, IDbTransaction? transaction = null);
        Task<IEnumerable<Installment>> GetAllOpenByCustomerIdAsync(Guid customerId, IDbTransaction? transaction = null);
    }
}
