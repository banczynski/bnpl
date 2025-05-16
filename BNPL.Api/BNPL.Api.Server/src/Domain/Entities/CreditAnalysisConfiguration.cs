using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("credit_analysis_configuration")]
    public sealed class CreditAnalysisConfiguration : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid? AffiliateId { get; init; }
        public decimal MinApprovedPercentage { get; set; } 
        public decimal MaxApprovedPercentage { get; set; }
        public decimal RejectionThreshold { get; set; } 
    }
}
