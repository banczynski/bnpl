using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class PartnerMapper
    {
        public static Partner ToEntity(this CreatePartnerRequest request, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                Name = request.Name,
                TaxId = request.TaxId,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Partner entity, UpdatePartnerRequest request, DateTime now, string user)
        {
            entity.Name = request.Name;
            entity.TaxId = request.TaxId;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static PartnerDto ToDto(this Partner entity)
            => new(
                entity.Id,
                entity.Name,
                entity.TaxId,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.CreatedBy,
                entity.UpdatedBy
            );
    }
}
