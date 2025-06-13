using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Kyc
{
    public sealed record KycDto(
        Guid Id,
        Guid CustomerId,
        DocumentType? DocumentType,
        string? DocumentNumber,
        string? DocumentImageUrl,
        string? SelfieImageUrl,
        bool OcrValidated,
        bool FaceMatchValidated,
        KycStatus Status,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        Guid CreatedBy,
        Guid UpdatedBy
    );
}
