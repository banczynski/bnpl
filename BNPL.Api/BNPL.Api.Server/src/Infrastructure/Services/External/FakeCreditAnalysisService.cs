using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakeCreditAnalysisService(
        ICreditAnalysisConfigurationService configService
    ) : ICreditAnalysisService
    {
        public async Task<CreditAnalysisResult> AnalyzeAsync(Guid partnerId, Guid? affiliateId, string customerTaxId, decimal requestedAmount)
        {
            var config = await configService.GetEffectiveConfigAsync(partnerId, affiliateId);
            var random = new Random();
            var score = (decimal)random.NextDouble();

            var approved = score >= config.RejectionThreshold;

            var percentage = approved
                ? score * (config.MaxApprovedPercentage - config.MinApprovedPercentage) + config.MinApprovedPercentage
                : 0m;

            var approvedAmount = Math.Round(requestedAmount * percentage, 2);

            return new CreditAnalysisResult(
                Decision: approved ? CreditAnalysisStatus.Approved : CreditAnalysisStatus.Rejected,
                ApprovedAmount: approvedAmount,
                MaxInstallments: 6,
                MonthlyInterestRate: 0.035m,
                Score: score
            );
        }
    }
}
