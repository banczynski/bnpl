using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("invoice")]
    public sealed class Invoice : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("customer_id")]
        public Guid CustomerId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("due_date")]
        public DateTime DueDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("status")]
        public InvoiceStatus Status { get; set; }

        [Column("is_individual")]
        public bool IsIndividual { get; set; }

        public void MarkAsOverdue(DateTime now, Guid user) => SetStatus(InvoiceStatus.Overdue, now, user);
        public void MarkAsPaid(DateTime now, Guid user) => SetStatus(InvoiceStatus.Paid, now, user);
        public void MarkAsPending(DateTime now, Guid user) => SetStatus(InvoiceStatus.Pending, now, user);

        private void SetStatus(InvoiceStatus newStatus, DateTime now, Guid user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
