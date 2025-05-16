using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("customer")]
    public sealed class Customer : BaseEntity
    {
        public string TaxId { get; init; } = default!;
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
