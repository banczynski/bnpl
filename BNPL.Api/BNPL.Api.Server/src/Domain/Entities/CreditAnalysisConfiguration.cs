using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("credit_analysis_configuration")]
    public sealed class CreditAnalysisConfiguration : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid? AffiliateId { get; init; }

        [Column("min_approved_percentage")]
        public decimal MinApprovedPercentage { get; set; }

        [Column("max_approved_percentage")]
        public decimal MaxApprovedPercentage { get; set; }

        [Column("rejection_threshold")]
        public decimal RejectionThreshold { get; set; }

        [Column("max_credit_amount")]
        public decimal MaxCreditAmount { get; set; }

        [Column("max_installments")]
        public int MaxInstallments { get; set; }

        [Column("monthly_interest_rate")]
        public decimal MonthlyInterestRate { get; set; }
    }
}
