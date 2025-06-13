using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class CreateSimulationUseCase(
        ISimulationRepository simulationRepository,
        IAffiliateRepository affiliateRepository,
        EvaluateCustomerCreditUseCase evaluateCustomerCreditUseCase,
        IInstallmentCalculator installmentCalculator,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<SimulationWithInstallmentsResponse, string>> ExecuteAsync(Guid affiliateId, CreateSimulationRequest request)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(affiliateId);
                if (partnerId.GetValueOrDefault() == Guid.Empty)
                    return Result<SimulationWithInstallmentsResponse, string>.Fail("Affiliate not found.");

                var result = await evaluateCustomerCreditUseCase.ExecuteAsync(partnerId.Value, affiliateId, request.CustomerTaxId, scope.Transaction);

                if (result is Result<CreditAnalysisResult, string>.Failure fail)
                    return Result<SimulationWithInstallmentsResponse, string>.Fail(fail.Error);

                if (result is not Result<CreditAnalysisResult, string>.Success success)
                    return Result<SimulationWithInstallmentsResponse, string>.Fail("Unexpected result state");

                var decision = success.Value;

                if (request.RequestedAmount > decision.ApprovedLimit)
                    return Result<SimulationWithInstallmentsResponse, string>.Fail("Requested amount exceeds approved credit limit.");

                var entity = request.ToEntity(
                    partnerId: partnerId.Value,
                    affiliateId: affiliateId,
                    approvedAmount: decision.ApprovedLimit,
                    maxInstallments: decision.MaxInstallments,
                    interestRate: decision.MonthlyInterestRate,
                    user: userContext.GetRequiredUserId()
                );

                await simulationRepository.InsertAsync(entity);

                var installmentOptions = installmentCalculator.Calculate(
                    amount: request.RequestedAmount,
                    maxInstallments: decision.MaxInstallments,
                    monthlyInterestRate: decision.MonthlyInterestRate
                );

                var affordableOptions = installmentOptions
                    .Where(o => o.Total <= decision.ApprovedLimit)
                    .ToList();

                if (affordableOptions.Count == 0)
                    return Result<SimulationWithInstallmentsResponse, string>.Fail("No installment option fits the approved credit limit.");

                scope.Commit();

                return Result<SimulationWithInstallmentsResponse, string>.Ok(new SimulationWithInstallmentsResponse
                {
                    Simulation = entity.ToResponse(),
                    Installments = affordableOptions
                });
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
