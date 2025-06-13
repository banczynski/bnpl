using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("partner")]
    public sealed class Partner : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("tax_id")]
        public string TaxId { get; set; } = default!;
    }
}
