using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal")]
    public sealed class Proposal : BaseEntity
    {
        public Guid PartnerId { get; init; }
        public Guid AffiliateId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerTaxId { get; init; } = default!;
        public Guid SimulationId { get; init; }
        public decimal RequestedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public int Installments { get; set; }
        public decimal MonthlyInterestRate { get; set; }
        public ProposalStatus Status { get; set; } = ProposalStatus.Created;

        public void MarkAsAwaitingSignature(DateTime now, string user) => SetStatus(ProposalStatus.AwaitingSignature, now, user);
        public void MarkAsCancelled(DateTime now, string user) => SetStatus(ProposalStatus.Cancelled, now, user);
        public void MarkAsDisbursed(DateTime now, string user) => SetStatus(ProposalStatus.Disbursed, now, user);
        public void MarkAsFinalized(DateTime now, string user) => SetStatus(ProposalStatus.Finalized, now, user);
        public void MarkAsFormalized(DateTime now, string user) => SetStatus(ProposalStatus.Formalized, now, user);
        public void MarkAsRejected(DateTime now, string user) => SetStatus(ProposalStatus.Rejected, now, user);
        public void MarkAsSigned(DateTime now, string user) => SetStatus(ProposalStatus.Signed, now, user);

        private void SetStatus(ProposalStatus newStatus, DateTime now, string user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }

}
