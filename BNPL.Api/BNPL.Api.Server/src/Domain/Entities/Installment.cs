using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("installment")]
    public sealed class Installment : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("proposal_id")]
        public Guid? ProposalId { get; init; }

        [Column("customer_id")]
        public Guid CustomerId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("sequence")]
        public int Sequence { get; init; }

        [Column("due_date")]
        public DateTime DueDate { get; init; }

        [Column("amount")]
        public decimal Amount { get; init; }

        [Column("status")]
        public InstallmentStatus Status { get; set; }

        [Column("invoice_id")]
        public Guid? InvoiceId { get; set; }

        [Column("payment_id")]
        public Guid? PaymentId { get; set; } 

        public void MarkAsOverdue(DateTime now, Guid user) => SetStatus(InstallmentStatus.Overdue, now, user);
        public void MarkAsPaid(DateTime now, Guid user) => SetStatus(InstallmentStatus.Paid, now, user);
        public void MarkAsPending(DateTime now, Guid user) => SetStatus(InstallmentStatus.Pending, now, user);

        private void SetStatus(InstallmentStatus newStatus, DateTime now, Guid user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
