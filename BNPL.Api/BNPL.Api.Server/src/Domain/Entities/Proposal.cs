using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal")]
    public sealed class Proposal : BaseEntity
    {
        [Column("partner_id")]
        public Guid PartnerId { get; init; }

        [Column("affiliate_id")]
        public Guid AffiliateId { get; init; }

        [Column("customer_id")]
        public Guid CustomerId { get; init; }

        [Column("customer_tax_id")]
        public string CustomerTaxId { get; init; } = default!;

        [Column("simulation_id")]
        public Guid SimulationId { get; init; }

        [Column("requested_amount")]
        public decimal RequestedAmount { get; set; }

        [Column("total_with_charges")]
        public decimal TotalWithCharges { get; set; }

        [Column("term")]
        public int Term { get; set; }

        [Column("monthly_interest_rate")]
        public decimal MonthlyInterestRate { get; set; }

        [Column("status")]
        public ProposalStatus Status { get; set; } 

        [Column("preferred_due_day")]
        public int PreferredDueDay { get; set; }

        public void MarkAsApproved(DateTime now, Guid user) => SetStatus(ProposalStatus.Approved, now, user);
        public void MarkAsAwaitingSignature(DateTime now, Guid user) => SetStatus(ProposalStatus.AwaitingSignature, now, user);
        public void MarkAsSigned(DateTime now, Guid user) => SetStatus(ProposalStatus.Signed, now, user);
        public void MarkAsActive(DateTime now, Guid user) => SetStatus(ProposalStatus.Active, now, user);
        public void MarkAsFinalized(DateTime now, Guid user) => SetStatus(ProposalStatus.Finalized, now, user);
        public void MarkAsCancelled(DateTime now, Guid user) => SetStatus(ProposalStatus.Cancelled, now, user);

        private void SetStatus(ProposalStatus newStatus, DateTime now, Guid user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
