using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class SimulationMapper
    {
        public static Simulation ToEntity(this CreateSimulationRequest request, decimal approvedAmount, int maxInstallments, decimal interestRate, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                CustomerTaxId = request.CustomerTaxId,
                RequestedAmount = request.RequestedAmount,
                ApprovedAmount = approvedAmount,
                MaxInstallments = maxInstallments,
                MonthlyInterestRate = interestRate,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static SimulationResponse ToResponse(this Simulation entity)
            => new(
                entity.Id,
                entity.ApprovedAmount,
                entity.MaxInstallments,
                entity.MonthlyInterestRate
            );
    }
}
