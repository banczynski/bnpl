using BNPL.Api.Server.src.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("proposal_signature")]
    public sealed class ProposalSignature
    {
        public Guid ProposalId { get; init; }
        public string ExternalSignatureId { get; set; } = default!;
        public SignatureStatus Status { get; set; } = SignatureStatus.Pending;
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; init; } = default!;
        public string UpdatedBy { get; set; } = default!;
    }
}
