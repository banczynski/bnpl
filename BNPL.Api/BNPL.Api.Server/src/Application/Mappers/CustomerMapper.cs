using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToEntity(this CreateCustomerRequest request, Guid partnerId, Guid affiliateId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                TaxId = request.TaxId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Customer entity, UpdateCustomerRequest request, DateTime now, Guid user)
        {
            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static CustomerDto ToDto(this Customer c)
            => new(
                c.Code,
                c.PartnerId,
                c.AffiliateId,
                c.TaxId,
                c.Name,
                c.Email,
                c.Phone,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt,
                c.CreatedBy,
                c.UpdatedBy
            );
    }
}
