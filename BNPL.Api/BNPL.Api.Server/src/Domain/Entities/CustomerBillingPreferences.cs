using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("customer_billing_preferences")]
    public sealed class CustomerBillingPreferences : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("customer_id")]
        public Guid CustomerId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("invoice_due_day")]
        public int InvoiceDueDay { get; set; }

        [Column("consolidated_invoice_enabled")]
        public bool ConsolidatedInvoiceEnabled { get; set; }

        public void UpdateInvoiceDueDay(int dueDay, DateTime now, Guid user)
        {
            InvoiceDueDay = dueDay;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
