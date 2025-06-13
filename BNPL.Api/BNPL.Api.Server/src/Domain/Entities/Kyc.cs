using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BNPL.Api.Server.src.Domain.Entities
{
    [Table("kyc")]
    public sealed class Kyc : BaseEntity
    {
        [Column("customer_id")]
        public Guid CustomerId { get; init; }

        [Column("document_type")]
        public DocumentType? DocumentType { get; set; }

        [Column("document_number")]
        public string? DocumentNumber { get; set; }

        [Column("document_image_url")]
        public string? DocumentImageUrl { get; set; }

        [Column("selfie_image_url")]
        public string? SelfieImageUrl { get; set; }

        [Column("ocr_validated")]
        public bool OcrValidated { get; set; }

        [Column("face_match_validated")]
        public bool FaceMatchValidated { get; set; }

        [Column("status")]
        public KycStatus Status { get; set; } = KycStatus.Pending;

        public void MarkAsManualReview(DateTime now, Guid user) => SetStatus(KycStatus.ManualReview, now, user);
        public void MarkAsPending(DateTime now, Guid user) => SetStatus(KycStatus.Pending, now, user);
        public void MarkAsRejected(DateTime now, Guid user) => SetStatus(KycStatus.Rejected, now, user);
        public void MarkAsValidated(DateTime now, Guid user) => SetStatus(KycStatus.Validated, now, user);

        private void SetStatus(KycStatus newStatus, DateTime now, Guid user)
        {
            Status = newStatus;
            UpdatedAt = now;
            UpdatedBy = user;
        }
    }
}
