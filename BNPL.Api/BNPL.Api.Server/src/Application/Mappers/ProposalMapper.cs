using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class ProposalMapper
    {
        public static Proposal ToEntity(this CreateProposalRequest request, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                CustomerId = request.CustomerId,
                CustomerTaxId = request.CustomerTaxId,
                SimulationId = request.SimulationId,
                RequestedAmount = request.RequestedAmount,
                ApprovedAmount = request.ApprovedAmount,
                Installments = request.Installments,
                MonthlyInterestRate = request.MonthlyInterestRate,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Proposal entity, UpdateProposalRequest request, DateTime now, string user)
        {
            entity.RequestedAmount = request.RequestedAmount;
            entity.ApprovedAmount = request.ApprovedAmount;
            entity.Installments = request.Installments;
            entity.MonthlyInterestRate = request.MonthlyInterestRate;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static ProposalDto ToDto(this Proposal entity)
            => new(
                entity.Id,
                entity.PartnerId,
                entity.AffiliateId,
                entity.CustomerId,
                entity.CustomerTaxId,
                entity.SimulationId,
                entity.RequestedAmount,
                entity.ApprovedAmount,
                entity.Installments,
                entity.MonthlyInterestRate,
                entity.Status,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.CreatedBy,
                entity.UpdatedBy
            );
    }
}
