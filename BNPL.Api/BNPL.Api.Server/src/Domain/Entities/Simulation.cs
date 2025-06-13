using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("simulation")]
    public sealed class Simulation : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("requested_amount")]
        public decimal RequestedAmount { get; init; }

        [Column("approved_limit")]
        public decimal ApprovedLimit { get; init; }

        [Column("max_installments")]
        public int MaxInstallments { get; init; }

        [Column("monthly_interest_rate")]
        public decimal MonthlyInterestRate { get; init; }
    }
}
