using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("partner")]
    public sealed class Partner : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string TaxId { get; set; } = default!;
    }
}
