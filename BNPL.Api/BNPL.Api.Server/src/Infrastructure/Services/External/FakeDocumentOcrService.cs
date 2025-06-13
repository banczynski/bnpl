using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.DTOs.Kyc;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakeDocumentOcrService : IDocumentOcrService
    {
        public Task<OcrExtractionResult> AnalyzeAsync(Uri documentImageUrl)
        {
            return Task.FromResult(new OcrExtractionResult(
                DocumentNumber: "123456789",
                Name: "João da Silva",
                TaxId: "12345678900",
                BirthDate: new DateTime(1990, 5, 1),
                ExpirationDate: new DateTime(2027, 12, 31)
            ));
        }
    }
}
