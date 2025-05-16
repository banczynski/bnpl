using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class AffiliateMapper
    {
        public static Affiliate ToEntity(this CreateAffiliateRequest request, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                Name = request.Name,
                TaxId = request.TaxId,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Affiliate entity, UpdateAffiliateRequest request, DateTime now, string user)
        {
            entity.Name = request.Name;
            entity.TaxId = request.TaxId;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static AffiliateDto ToDto(this Affiliate entity)
            => new(
                entity.Id,
                entity.PartnerId,
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
