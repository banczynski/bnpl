using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("affiliate")]
    public sealed class Affiliate : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public string Name { get; set; } = default!;
        public string TaxId { get; set; } = default!;
    }
}
