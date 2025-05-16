using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("installment")]
    public sealed class Installment : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public Guid? ProposalId { get; init; }
        public Guid? RenegotiationId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerTaxId { get; init; } = default!;        
        public int Sequence { get; init; }
        public DateTime DueDate { get; init; }
        public decimal Amount { get; init; }
        public InstallmentStatus Status { get; set; } = InstallmentStatus.Pending;
        public Guid? InvoiceId { get; set; }

        public void MarkAsOverdue(DateTime now, string user) => SetStatus(InstallmentStatus.Overdue, now, user);
        public void MarkAsPaid(DateTime now, string user) => SetStatus(InstallmentStatus.Paid, now, user);
        public void MarkAsPending(DateTime now, string user) => SetStatus(InstallmentStatus.Pending, now, user);

        private void SetStatus(InstallmentStatus newStatus, DateTime now, string user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
