using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class AffiliateMapper
    {
        public static Affiliate ToEntity(this CreateAffiliateRequest request, Guid partnerId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId,
                Name = request.Name,
                TaxId = request.TaxId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Affiliate entity, UpdateAffiliateRequest request, DateTime now, Guid user)
        {
            entity.Name = request.Name;
            entity.TaxId = request.TaxId;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static AffiliateDto ToDto(this Affiliate a)
            => new(
                a.Code,
                a.PartnerId,
                a.Name,
                a.TaxId,
                a.IsActive,
                a.CreatedAt,
                a.UpdatedAt,
                a.CreatedBy,
                a.UpdatedBy
            );
    }
}
