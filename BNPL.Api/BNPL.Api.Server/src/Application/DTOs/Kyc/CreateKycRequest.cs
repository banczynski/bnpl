using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Kyc
{
    public sealed record CreateKycRequest(
        Guid CustomerId,
        DocumentType DocumentType,
        string DocumentNumber,
        string DocumentImageUrl,
        string SelfieImageUrl
    );
}
