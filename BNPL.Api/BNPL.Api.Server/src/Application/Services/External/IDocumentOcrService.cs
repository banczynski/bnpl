using BNPL.Api.Server.src.Application.DTOs.Kyc;

namespace BNPL.Api.Server.src.Application.Services.External
{
    public interface IDocumentOcrService
    {
        Task<OcrExtractionResult> AnalyzeAsync(Uri documentImageUrl);
    }
}
