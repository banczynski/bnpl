using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("financial_charges_configuration")]
    public sealed class FinancialChargesConfiguration : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid? AffiliateId { get; init; }
        public decimal InterestRate { get; set; } // Mora diária
        public decimal ChargesRate { get; set; } // Multa
        public decimal LateFeeRate { get; set; } // Valor fixo por atraso (opcional)
        public int GraceDays { get; set; } = 0;
        public bool ApplyCompoundInterest { get; set; } = false;
    }
}
