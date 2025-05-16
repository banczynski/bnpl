using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("customer_credit_limit")]
    public sealed class CustomerCreditLimit : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public string CustomerTaxId { get; init; } = default!;
        public decimal ApprovedLimit { get; set; }
        public decimal UsedLimit { get; set; }

        [Write(false)]
        public decimal AvailableLimit => ApprovedLimit - UsedLimit;

        public void IncreaseUsedLimit(decimal value, DateTime now, string user)
        {
            UsedLimit += value;
            UpdatedAt = now;
            UpdatedBy = user;
        }

        public void DecreaseUsedLimit(decimal value, DateTime now, string user)
        {
            UsedLimit -= value;
            if (UsedLimit < 0) UsedLimit = 0;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
