using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class ProposalItemMapper
    {
        public static ProposalItem ToEntity(this CreateProposalItemRequest request, Guid proposalId, DateTime now, string user)
            => new()
            {
                Id = Guid.NewGuid(),
                ProposalId = proposalId,
                ProductId = request.ProductId,
                Description = request.Description,
                Amount = request.Amount,
                AffiliateId = request.AffiliateId,
                Returned = false,
                ReturnReason = null,
                ReturnedAt = null,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static ProposalItemDto ToDto(this ProposalItem item)
            => new(
                item.ProposalId,
                item.ProductId,
                item.Description,
                item.Amount,
                item.AffiliateId,
                item.Returned,
                item.ReturnReason,
                item.ReturnedAt
            );
    }
}
