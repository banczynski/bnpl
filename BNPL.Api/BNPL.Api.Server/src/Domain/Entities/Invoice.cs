using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("invoice")]
    public sealed class Invoice : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerTaxId { get; init; } = default!;
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        public void MarkAsOverdue(DateTime now, string user) => SetStatus(InvoiceStatus.Overdue, now, user);
        public void MarkAsPaid(DateTime now, string user) => SetStatus(InvoiceStatus.Paid, now, user);
        public void MarkAsPending(DateTime now, string user) => SetStatus(InvoiceStatus.Pending, now, user);

        private void SetStatus(InvoiceStatus newStatus, DateTime now, string user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
