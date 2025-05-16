using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task InsertAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true);
        Task<IEnumerable<Customer>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true);
    }
}
