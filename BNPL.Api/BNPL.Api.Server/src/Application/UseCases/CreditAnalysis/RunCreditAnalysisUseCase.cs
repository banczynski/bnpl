using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed record RunCreditAnalysisRequestUseCase(Guid PartnerId, Guid AffiliateId, string CustomerTaxId);

    public sealed class RunCreditAnalysisUseCase(
        EvaluateCustomerCreditUseCase evaluateCustomerCreditUseCase,
        IUnitOfWork unitOfWork
    ) : IUseCase<RunCreditAnalysisRequestUseCase, Result<CreditAnalysisResult, Error>>
    {
        public async Task<Result<CreditAnalysisResult, Error>> ExecuteAsync(RunCreditAnalysisRequestUseCase request)
        {
            var result = await evaluateCustomerCreditUseCase.ExecuteAsync(
                request.PartnerId,
                request.AffiliateId,
                request.CustomerTaxId,
                unitOfWork.Transaction);

            if (result.TryGetError(out var error))
                return Result<CreditAnalysisResult, Error>.Fail(error!);

            if (result.TryGetSuccess(out var successValue))
                return Result<CreditAnalysisResult, Error>.Ok(successValue);

            return Result<CreditAnalysisResult, Error>.Fail(DomainErrors.General.Unexpected);
        }
    }
}