using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal_item")]
    public sealed class ProposalItem : BaseEntity
    {
        [Column("proposal_id")]
        public Guid ProposalId { get; init; }

        [Column("description")]
        public string Description { get; init; } = default!;

        [Column("amount")]
        public decimal Amount { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("returned")]
        public bool Returned { get; set; }

        [Column("return_reason")]
        public string? ReturnReason { get; set; }

        [Column("returned_at")] 
        public DateTime? ReturnedAt { get; set; }

        [Column("return_confirmed_at")]
        public DateTime? ReturnConfirmedAt { get; set; }
    }
}
