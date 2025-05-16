using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakeSignatureService : ISignatureService
    {
        public Task<Uri> GenerateSignatureLinkAsync(SignatureRequest request)
        {
            var fakeUrl = $"https://signature.example.com/proposals/{request.ProposalId}";
            return Task.FromResult(new Uri(fakeUrl));
        }

        public Task<SignatureStatus> CheckStatusAsync(string externalSignatureId)
        {
            return Task.FromResult(SignatureStatus.Signed);
        }
    }
}
