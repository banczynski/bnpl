using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using Core.Models;
using System.Data;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class EvaluateCustomerCreditUseCase(
        ICreditAnalysisService creditAnalysisService,
        UpsertCustomerCreditLimitUseCase creditLimitUseCase
    )
    {
        public async Task<Result<CreditAnalysisResult, Error>> ExecuteAsync(
            Guid partnerId,
            Guid affiliateId,
            string customerTaxId,
            IDbTransaction transaction)
        {
            var decision = await creditAnalysisService.AnalyzeAsync(partnerId, affiliateId, customerTaxId);

            if (decision.Decision != Domain.Enums.CreditAnalysisStatus.Approved || decision.ApprovedLimit <= 0)
                return Result<CreditAnalysisResult, Error>.Fail(DomainErrors.CreditAnalysis.CreditDenied);

            var creditLimitRequest = new CreditLimitUpsertRequest(
                PartnerId: partnerId,
                AffiliateId: affiliateId,
                CustomerTaxId: customerTaxId,
                ApprovedLimit: decision.ApprovedLimit
            );

            var creditLimitResult = await creditLimitUseCase.ExecuteAsync(creditLimitRequest, transaction);

            if (creditLimitResult.TryGetError(out var error))
                return Result<CreditAnalysisResult, Error>.Fail(error!);

            return Result<CreditAnalysisResult, Error>.Ok(decision);
        }
    }
}