using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class KycMapper
    {
        public static Kyc ToEntity(this CreateKycRequest request, Guid customerId, Guid user)
            => new()
            {
                Code = Guid.NewGuid(),
                CustomerId = customerId,
                DocumentType = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                DocumentImageUrl = request.DocumentImageUrl,
                SelfieImageUrl = request.SelfieImageUrl,
                OcrValidated = false,
                FaceMatchValidated = false,
                Status = Domain.Enums.KycStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user,
                UpdatedBy = user
            };

        public static void UpdateEntity(this Kyc entity, UpdateKycRequest request, DateTime now, Guid user)
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

        public static KycDto ToDto(this Kyc k)
            => new(
                k.Code,
                k.CustomerId,
                k.DocumentType,
                k.DocumentNumber,
                k.DocumentImageUrl,
                k.SelfieImageUrl,
                k.OcrValidated,
                k.FaceMatchValidated,
                k.Status,
                k.CreatedAt,
                k.UpdatedAt,
                k.CreatedBy,
                k.UpdatedBy
            );
    }
}
