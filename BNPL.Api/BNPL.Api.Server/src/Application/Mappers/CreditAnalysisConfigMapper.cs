using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CreditAnalysisConfigMapper
    {
        public static CreditAnalysisConfiguration ToEntity(this CreateCreditAnalysisConfigRequest request, Guid partnerId, Guid? affiliateId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                MinApprovedPercentage = request.MinApprovedPercentage,
                MaxApprovedPercentage = request.MaxApprovedPercentage,
                RejectionThreshold = request.RejectionThreshold,
                MaxCreditAmount = request.MaxCreditAmount,
                MaxInstallments = request.MaxInstallments,
                MonthlyInterestRate = request.MonthlyInterestRate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this CreditAnalysisConfiguration entity, UpdateCreditAnalysisConfigRequest request, DateTime now, Guid user)
        {
            entity.MinApprovedPercentage = request.MinApprovedPercentage;
            entity.MaxApprovedPercentage = request.MaxApprovedPercentage;
            entity.RejectionThreshold = request.RejectionThreshold;
            entity.MaxCreditAmount = request.MaxCreditAmount;
            entity.MaxInstallments = request.MaxInstallments;
            entity.MonthlyInterestRate = request.MonthlyInterestRate;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static CreditAnalysisConfigDto ToDto(this CreditAnalysisConfiguration c)
            => new(
                c.Code,
                c.PartnerId,
                c.AffiliateId,
                c.MinApprovedPercentage,
                c.MaxApprovedPercentage,
                c.RejectionThreshold,
                c.MaxCreditAmount,
                c.MaxInstallments,
                c.MonthlyInterestRate,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt,
                c.CreatedBy,
                c.UpdatedBy
            );
    }
}
