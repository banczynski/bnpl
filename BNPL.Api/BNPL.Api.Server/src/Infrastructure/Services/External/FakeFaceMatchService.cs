using BNPL.Api.Server.src.Application.Services.External;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakeFaceMatchService : IFaceMatchService
    {
        public Task<bool> ValidateAsync(Uri documentImageUrl, Uri selfieImageUrl)
        {
            return Task.FromResult(true);
        }
    }
}
