using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class FinancialChargesConfigMapper
    {
        public static FinancialChargesConfiguration ToEntity(this CreateFinancialChargesConfigRequest request, Guid partnerId, Guid? affiliateId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                InterestRate = request.InterestRate,
                ChargesRate = request.ChargesRate,
                LateFeeRate = request.LateFeeRate,
                GraceDays = request.GraceDays,
                ApplyCompoundInterest = request.ApplyCompoundInterest,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this FinancialChargesConfiguration entity, UpdateFinancialChargesConfigRequest request, DateTime now, Guid user)
        {
            entity.InterestRate = request.InterestRate;
            entity.ChargesRate = request.ChargesRate;
            entity.LateFeeRate = request.LateFeeRate;
            entity.GraceDays = request.GraceDays;
            entity.ApplyCompoundInterest = request.ApplyCompoundInterest;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static FinancialChargesConfigDto ToDto(this FinancialChargesConfiguration f)
            => new(
                f.Code,
                f.PartnerId,
                f.AffiliateId,
                f.InterestRate,
                f.ChargesRate,
                f.LateFeeRate,
                f.GraceDays,
                f.ApplyCompoundInterest,
                f.IsActive,
                f.CreatedAt,
                f.UpdatedAt,
                f.CreatedBy,
                f.UpdatedBy
            );
    }
}
