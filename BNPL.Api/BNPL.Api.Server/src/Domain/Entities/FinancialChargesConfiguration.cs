using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("financial_charges_configuration")]
    public sealed class FinancialChargesConfiguration : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid? AffiliateId { get; init; }

        [Column("interest_rate")]
        public decimal InterestRate { get; set; }

        [Column("charges_rate")]
        public decimal ChargesRate { get; set; } 

        [Column("late_fee_rate")]
        public decimal LateFeeRate { get; set; } 

        [Column("grace_days")]
        public int GraceDays { get; set; } = 0;

        [Column("apply_compound_interest")]
        public bool ApplyCompoundInterest { get; set; } = false;
    }
}
