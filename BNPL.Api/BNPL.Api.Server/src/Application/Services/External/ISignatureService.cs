using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Services.External
{
    public interface ISignatureService
    {
        Task<Uri> GenerateSignatureLinkAsync(SignatureRequest request);
        Task<SignatureStatus> CheckStatusAsync(string externalSignatureId);
    }
}
