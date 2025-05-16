using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToEntity(this CreateCustomerRequest request, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                TaxId = request.TaxId,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Customer entity, UpdateCustomerRequest request, DateTime now, string user)
        {
            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static CustomerDto ToDto(this Customer entity)
            => new(
                entity.Id,
                entity.PartnerId,
                entity.AffiliateId,
                entity.TaxId,
                entity.Name,
                entity.Email,
                entity.Phone,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.CreatedBy,
                entity.UpdatedBy
            );
    }
}
