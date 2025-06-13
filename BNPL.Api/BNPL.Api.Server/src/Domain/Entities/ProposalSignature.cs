using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal_signature")]
    public sealed class ProposalSignature : BaseEntity
    {
        [Column("proposal_id")]
        public Guid ProposalId { get; init; }

        [Column("external_signature_id")]
        public string ExternalSignatureId { get; set; } = default!;

        [Column("destination")]
        public string Destination { get; set; } = default!;

        [Column("status")]
        public SignatureStatus Status { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        public void MarkAsSigned(DateTime now, Guid user) => SetStatus(SignatureStatus.Signed, now, user);

        private void SetStatus(SignatureStatus newStatus, DateTime now, Guid user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
