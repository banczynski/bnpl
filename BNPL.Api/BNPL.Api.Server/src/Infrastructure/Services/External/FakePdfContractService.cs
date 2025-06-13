using BNPL.Api.Server.src.Application.Abstractions.Storage;
using BNPL.Api.Server.src.Application.DTOs.Contract;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakePdfContractService : IPdfContractService
    {
        public Task<Uri> GenerateFinalDocumentAsync(ContractGenerationRequest request)
        {
            var fakeUrl = $"https://fake-pdf-service.com/contracts/{request.ProposalId}/final.pdf";
            return Task.FromResult(new Uri(fakeUrl));
        }
    }
}
