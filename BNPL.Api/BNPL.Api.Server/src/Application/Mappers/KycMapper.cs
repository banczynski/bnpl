using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class KycMapper
    {
        public static Kyc ToEntity(this CreateKycRequest request, DateTime now, string user)
            => new()
            {
                CustomerId = request.CustomerId,
                DocumentType = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                DocumentImageUrl = request.DocumentImageUrl,
                SelfieImageUrl = request.SelfieImageUrl,
                OcrValidated = false,
                FaceMatchValidated = false,
                Status = Domain.Enums.KycStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user
            };

        public static void UpdateEntity(this Kyc entity, UpdateKycRequest request, DateTime now, string user)
        {
            if (request.DocumentType.HasValue)
                entity.DocumentType = request.DocumentType;

            if (request.DocumentNumber is not null)
                entity.DocumentNumber = request.DocumentNumber;

            if (request.DocumentImageUrl is not null)
                entity.DocumentImageUrl = request.DocumentImageUrl;

            if (request.SelfieImageUrl is not null)
                entity.SelfieImageUrl = request.SelfieImageUrl;

            if (request.OcrValidated.HasValue)
                entity.OcrValidated = request.OcrValidated.Value;

            if (request.FaceMatchValidated.HasValue)
                entity.FaceMatchValidated = request.FaceMatchValidated.Value;

            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static KycDto ToDto(this Kyc entity)
            => new(
                entity.CustomerId,
                entity.DocumentType,
                entity.DocumentNumber,
                entity.DocumentImageUrl,
                entity.SelfieImageUrl,
                entity.OcrValidated,
                entity.FaceMatchValidated,
                entity.Status,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.CreatedBy,
                entity.UpdatedBy
            );
    }
}
