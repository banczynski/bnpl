using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class PartnerMapper
    {
        public static Partner ToEntity(this CreatePartnerRequest request, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                Name = request.Name,
                TaxId = request.TaxId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Partner entity, UpdatePartnerRequest request, DateTime now, Guid user)
        {
            entity.Name = request.Name;
            entity.TaxId = request.TaxId;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static PartnerDto ToDto(this Partner p)
            => new(
                p.Code,
                p.Name,
                p.TaxId,
                p.IsActive,
                p.CreatedAt,
                p.UpdatedAt,
                p.CreatedBy,
                p.UpdatedBy
            );
    }
}
