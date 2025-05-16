using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IInstallmentRepository
    {
        Task InsertManyAsync(IEnumerable<Installment> installments);
        Task<Installment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Installment>> GetByProposalIdAsync(Guid proposalId); 
        Task<IEnumerable<Installment>> GetByInvoiceIdAsync(Guid invoiceId);
        Task<IEnumerable<Installment>> GetPendingByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Installment>> GetPendingDueInDaysAsync(int days);
        Task<IEnumerable<Installment>> GetPendingByIdsAsync(IEnumerable<Guid> ids);
        Task UpdateAsync(Installment installment);
    }
}
