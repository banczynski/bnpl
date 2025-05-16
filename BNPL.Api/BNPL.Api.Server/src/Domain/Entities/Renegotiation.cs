using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("renegotiation")]
    public sealed class Renegotiation : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerTaxId { get; init; } = default!;
        public List<Guid> OriginalInvoiceIds { get; init; } = [];
        public List<Guid> OriginalInstallmentIds { get; init; } = [];
        public decimal OriginalTotalAmount { get; init; }
        public decimal NewTotalAmount { get; init; }
        public int NewInstallments { get; init; }
        public decimal MonthlyInterestRate { get; init; }

        public RenegotiationStatus Status { get; set; } = RenegotiationStatus.Pending;

        public void MarkAsCancelled(DateTime now, string user) => SetStatus(RenegotiationStatus.Cancelled, now, user);
        public void MarkAsConfirmed(DateTime now, string user) => SetStatus(RenegotiationStatus.Confirmed, now, user);
        public void MarkAsPending(DateTime now, string user) => SetStatus(RenegotiationStatus.Pending, now, user);

        private void SetStatus(RenegotiationStatus newStatus, DateTime now, string user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
