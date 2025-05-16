using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal_item")]
    public sealed class ProposalItem : BaseEntity
    {
        public Guid ProposalId { get; init; }
        public Guid ProductId { get; init; }
        public string Description { get; init; } = default!;
        public decimal Amount { get; init; }
        public Guid AffiliateId { get; init; }
        public bool Returned { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
