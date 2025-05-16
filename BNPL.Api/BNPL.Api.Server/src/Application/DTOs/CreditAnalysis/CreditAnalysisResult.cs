using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public sealed record CreditAnalysisResult(
        CreditAnalysisStatus Decision,
        decimal ApprovedAmount,
        int MaxInstallments,
        decimal MonthlyInterestRate,
        decimal Score
    );
}
