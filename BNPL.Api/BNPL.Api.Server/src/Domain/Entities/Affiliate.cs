using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("affiliate")]
    public sealed class Affiliate : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("tax_id")]
        public string TaxId { get; set; } = default!;
    }
}
