using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using Dapper.Contrib.Extensions;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("kyc")]
    public sealed class Kyc : BaseEntity
    {
        public Guid CustomerId { get; init; }
        public DocumentType? DocumentType { get; set; } 
        public string? DocumentNumber { get; set; }
        public string? DocumentImageUrl { get; set; }
        public string? SelfieImageUrl { get; set; } 
        public bool OcrValidated { get; set; }
        public bool FaceMatchValidated { get; set; }
        public KycStatus Status { get; set; } = KycStatus.Pending;

        public void MarkAsManualReview(DateTime now, string user) => SetStatus(KycStatus.ManualReview, now, user);
        public void MarkAsPending(DateTime now, string user) => SetStatus(KycStatus.Pending, now, user);
        public void MarkAsRejected(DateTime now, string user) => SetStatus(KycStatus.Rejected, now, user);
        public void MarkAsValidated(DateTime now, string user) => SetStatus(KycStatus.Validated, now, user);

        private void SetStatus(KycStatus newStatus, DateTime now, string user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }        
    }
}
