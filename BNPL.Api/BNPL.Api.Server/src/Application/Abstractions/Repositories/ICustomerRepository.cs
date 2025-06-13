using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Guid?> GetIdByTaxIdAsync(string taxId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetByTaxIdAsync(string taxId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetActivesByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetAllByAffiliateIdAsync(Guid affiliateId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetActivesByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetAllByPartnerIdAsync(Guid partnerId, IDbTransaction? transaction = null);
    }
}