using BNPL.Api.Server.src.Domain.Entities;
using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Repositories
{
    public interface ICustomerRepository
    {
        IDbConnection Connection { get; }
        Task InsertAsync(Customer customer, IDbTransaction? transaction = null);
        Task UpdateAsync(Customer customer, IDbTransaction? transaction = null);
        Task InactivateAsync(Guid id, Guid updatedBy, DateTime updatedAt, IDbTransaction? transaction = null);
        Task<Customer?> GetByIdAsync(Guid id, IDbTransaction? transaction = null);
        Task<Guid?> GetIdByTaxIdAsync(string taxId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer?>> GetByTaxIdAsync(string taxId, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetByAffiliateIdAsync(Guid affiliateId, bool onlyActive = true, IDbTransaction? transaction = null);
        Task<IEnumerable<Customer>> GetByPartnerIdAsync(Guid partnerId, bool onlyActive = true, IDbTransaction? transaction = null);
    }
}
