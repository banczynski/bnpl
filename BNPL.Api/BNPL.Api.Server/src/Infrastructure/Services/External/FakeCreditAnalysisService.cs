using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    public sealed class FakeCreditAnalysisService(
        ICreditAnalysisConfigurationService configService
    ) : ICreditAnalysisService
    {
        public async Task<CreditAnalysisResult> AnalyzeAsync(Guid partnerId, Guid? affiliateId, string customerTaxId)
        {
            var config = await configService.GetEffectiveConfigAsync(partnerId, affiliateId);
            var random = new Random();
            var score = (decimal)random.NextDouble();

            var approved = score >= config.RejectionThreshold;

            var percentage = approved
                ? score * (config.MaxApprovedPercentage - config.MinApprovedPercentage) + config.MinApprovedPercentage
                : 0m;

            var approvedLimit = Math.Round(config.MaxCreditAmount * percentage, 2);

            return new CreditAnalysisResult(
                Decision: approved ? CreditAnalysisStatus.Approved : CreditAnalysisStatus.Rejected,
                ApprovedLimit: approvedLimit,
                MaxInstallments: config.MaxInstallments,
                MonthlyInterestRate: config.MonthlyInterestRate,
                Score: score
            );
        }
    }
}