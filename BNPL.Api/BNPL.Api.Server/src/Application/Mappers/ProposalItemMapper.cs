using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class ProposalItemMapper
    {
        public static ProposalItem ToEntity(this CreateProposalItemRequest request, Guid proposalId, Guid affiliateId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                ProposalId = proposalId,
                Description = request.Description,
                Amount = request.Amount,
                AffiliateId = affiliateId,
                Returned = false,
                ReturnReason = null,
                ReturnedAt = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static ProposalItemDto ToDto(this ProposalItem pi)
            => new(
                pi.Code,
                pi.ProposalId,
                pi.Description,
                pi.Amount,
                pi.AffiliateId,
                pi.Returned,
                pi.ReturnReason,
                pi.ReturnedAt
            );

        public static IEnumerable<ProposalItemDto> ToDtoList(this IEnumerable<ProposalItem> items)
            => [.. items.Select(pi => pi.ToDto())];
    }
}
