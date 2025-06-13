using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class SimulationMapper
    {
        public static Simulation ToEntity(this CreateSimulationRequest request, Guid partnerId, Guid affiliateId, decimal approvedAmount, int maxInstallments, decimal interestRate, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId,
                AffiliateId = affiliateId,
                CustomerTaxId = request.CustomerTaxId,
                RequestedAmount = request.RequestedAmount,
                ApprovedLimit = approvedAmount,
                MaxInstallments = maxInstallments,
                MonthlyInterestRate = interestRate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static SimulationResponse ToResponse(this Simulation entity)
            => new(
                entity.Code,
                entity.ApprovedLimit,
                entity.MaxInstallments,
                entity.MonthlyInterestRate
            );

        public static SimulationDto ToDto(this Simulation s) => new(
            Code: s.Code,
            PartnerId: s.PartnerId,
            AffiliateId: s.AffiliateId,
            CustomerTaxId: s.CustomerTaxId,
            RequestedAmount: s.RequestedAmount,
            ApprovedLimit: s.ApprovedLimit,
            MaxInstallments: s.MaxInstallments,
            MonthlyInterestRate: s.MonthlyInterestRate,
            CreatedAt: s.CreatedAt,
            UpdatedAt: s.UpdatedAt
        );

        public static IEnumerable<SimulationDto> ToDtoList(this IEnumerable<Simulation> simulations)
            => [.. simulations.Select(s => s.ToDto())];
    }
}
