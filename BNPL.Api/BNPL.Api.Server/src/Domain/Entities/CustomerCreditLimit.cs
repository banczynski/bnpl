using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("customer_credit_limit")]
    public sealed class CustomerCreditLimit : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("approved_limit")]
        public decimal ApprovedLimit { get; set; }

        [Column("used_limit")]
        public decimal UsedLimit { get; set; }

        public void IncreaseUsedLimit(decimal value, DateTime now, Guid user)
        {
            UsedLimit += value;
            UpdatedAt = now;
            UpdatedBy = user;
        }

        public void DecreaseUsedLimit(decimal value, DateTime now, Guid user)
        {
            UsedLimit -= value;
            if (UsedLimit < 0) UsedLimit = 0;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
