using BNPL.Api.Server.src.Application.DTOs.Signature;

namespace BNPL.Api.Server.src.Application.Abstractions.External
{
    public interface ISignatureService
    {
        Task<SignatureTokenResponse> GenerateSignatureTokenAsync(Guid proposalId, string customerDestination);
        Task<bool> ValidateSignatureTokenAsync(Guid proposalId, string sentTo);
    }
}
