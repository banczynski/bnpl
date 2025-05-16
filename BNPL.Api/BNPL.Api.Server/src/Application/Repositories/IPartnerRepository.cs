using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Repositories
{
    public interface IPartnerRepository
    {
        Task InsertAsync(Partner partner);
        Task UpdateAsync(Partner partner);
        Task InactivateAsync(Guid id, string updatedBy, DateTime updatedAt);
        Task<Partner?> GetByIdAsync(Guid id);
        Task<IEnumerable<Partner>> GetAllAsync(bool onlyActive = true);
    }
}
