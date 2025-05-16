using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CreditAnalysisConfigMapper
    {
        public static CreditAnalysisConfiguration ToEntity(this CreateCreditAnalysisConfigRequest request, DateTime now, string user)
            => new()
            {
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                MinApprovedPercentage = request.MinApprovedPercentage,
                MaxApprovedPercentage = request.MaxApprovedPercentage,
                RejectionThreshold = request.RejectionThreshold,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this CreditAnalysisConfiguration entity, UpdateCreditAnalysisConfigRequest request, DateTime now, string user)
        {
            entity.MinApprovedPercentage = request.MinApprovedPercentage;
            entity.MaxApprovedPercentage = request.MaxApprovedPercentage;
            entity.RejectionThreshold = request.RejectionThreshold;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static CreditAnalysisConfigDto ToDto(this CreditAnalysisConfiguration config)
            => new(
                config.PartnerId,
                config.AffiliateId,
                config.MinApprovedPercentage,
                config.MaxApprovedPercentage,
                config.RejectionThreshold,
                config.IsActive,
                config.CreatedAt,
                config.UpdatedAt,
                config.CreatedBy,
                config.UpdatedBy
            );
    }
}
