using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.CreditAnalysis
{
    public record class CreditAnalysisResult(
        CreditAnalysisStatus Decision,
        decimal ApprovedLimit,
        int MaxInstallments,
        decimal MonthlyInterestRate,
        decimal Score
    );
}
