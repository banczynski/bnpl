using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class ProposalMapper
    {
        public static void UpdateEntity(this Proposal entity, UpdateProposalRequest request, DateTime now, Guid user)
        {
            entity.RequestedAmount = request.RequestedAmount;
            entity.TotalWithCharges = request.TotalWithCharges;
            entity.Term = request.Installments;
            entity.MonthlyInterestRate = request.MonthlyInterestRate;
            entity.PreferredDueDay = request.PreferredDueDay;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static ProposalDto ToDto(this Proposal p)
            => new(
                p.Code,
                p.PartnerId,
                p.AffiliateId,
                p.CustomerId,
                p.CustomerTaxId,
                p.SimulationId,
                p.RequestedAmount,
                p.TotalWithCharges,
                p.Term,
                p.MonthlyInterestRate,
                p.PreferredDueDay,
                p.Status,
                p.IsActive,
                p.CreatedAt,
                p.UpdatedAt,
                p.CreatedBy,
                p.UpdatedBy
            );

        public static IEnumerable<ProposalDto> ToDtoList(this IEnumerable<Proposal> proposals)
            => [.. proposals.Select(p => p.ToDto())];
    }
}
