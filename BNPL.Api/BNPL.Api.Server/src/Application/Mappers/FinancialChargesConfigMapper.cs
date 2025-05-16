using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class FinancialChargesConfigMapper
    {
        public static FinancialChargesConfiguration ToEntity(this CreateFinancialChargesConfigRequest request, DateTime now, string user)
            => new()
            {
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                InterestRate = request.InterestRate,
                ChargesRate = request.ChargesRate,
                LateFeeRate = request.LateFeeRate,
                GraceDays = request.GraceDays,
                ApplyCompoundInterest = request.ApplyCompoundInterest,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this FinancialChargesConfiguration entity, UpdateFinancialChargesConfigRequest request, DateTime now, string user)
        {
            entity.InterestRate = request.InterestRate;
            entity.ChargesRate = request.ChargesRate;
            entity.LateFeeRate = request.LateFeeRate;
            entity.GraceDays = request.GraceDays;
            entity.ApplyCompoundInterest = request.ApplyCompoundInterest;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static FinancialChargesConfigDto ToDto(this FinancialChargesConfiguration entity)
            => new(
                entity.PartnerId,
                entity.AffiliateId,
                entity.InterestRate,
                entity.ChargesRate,
                entity.LateFeeRate,
                entity.GraceDays,
                entity.ApplyCompoundInterest,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.CreatedBy,
                entity.UpdatedBy
            );
    }
}
