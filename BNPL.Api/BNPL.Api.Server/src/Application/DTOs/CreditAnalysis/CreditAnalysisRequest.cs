namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreditAnalysisRequest(
        string CustomerTaxId,
        decimal RequestedAmount
    );
}
