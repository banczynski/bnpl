using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class RunCreditAnalysisUseCase(
        EvaluateCustomerCreditUseCase evaluateCustomerCreditUseCase,
        IUnitOfWork unitOfWork
    )
    {
        public async Task<Result<CreditAnalysisResult, string>> ExecuteAsync(Guid partnerId, Guid affiliateId, string customerTaxId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var result = await evaluateCustomerCreditUseCase.ExecuteAsync(partnerId, affiliateId, customerTaxId, scope.Transaction);

                if (result is Result<CreditAnalysisResult, string>.Failure fail)
                    return Result<CreditAnalysisResult, string>.Fail(fail.Error);

                if (result is not Result<CreditAnalysisResult, string>.Success success)
                    return Result<CreditAnalysisResult, string>.Fail("Unexpected result state");

                scope.Commit();
                return Result<CreditAnalysisResult, string>.Ok(success.Value);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
