using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;

namespace BNPL.Api.Server.src.Application.Abstractions.External
{
    public interface ICreditAnalysisService
    {
        Task<CreditAnalysisResult> AnalyzeAsync(Guid partnerId, Guid? affiliateId, string customerTaxId);
    }
}
