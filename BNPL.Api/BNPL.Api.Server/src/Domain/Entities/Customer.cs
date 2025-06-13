using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("customer")]
    public sealed class Customer : BaseEntity
    {
        [Column("tax_id")]
        public string TaxId { get; init; } = default!;

        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("email")]
        public string Email { get; set; } = default!;

        [Column("phone")]
        public string Phone { get; set; } = default!;
    }
}
