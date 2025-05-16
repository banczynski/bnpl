using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("simulation")]
    public sealed class Simulation : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public string CustomerTaxId { get; init; } = default!;
        public decimal RequestedAmount { get; init; }
        public decimal ApprovedAmount { get; init; }
        public int MaxInstallments { get; init; }
        public decimal MonthlyInterestRate { get; init; }
    }
}
