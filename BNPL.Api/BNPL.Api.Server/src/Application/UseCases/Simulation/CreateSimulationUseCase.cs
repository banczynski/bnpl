using BNPL.Api.Server.src.Application.Abstractions.Business;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed record CreateSimulationRequestUseCase(Guid AffiliateId, CreateSimulationRequest Dto);

    public sealed class CreateSimulationUseCase(
        ISimulationRepository simulationRepository,
        IAffiliateRepository affiliateRepository,
        EvaluateCustomerCreditUseCase evaluateCustomerCreditUseCase,
        IInstallmentCalculator installmentCalculator,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateSimulationRequestUseCase, Result<SimulationWithInstallmentsResponse, Error>>
    {
        public async Task<Result<SimulationWithInstallmentsResponse, Error>> ExecuteAsync(CreateSimulationRequestUseCase request)
        {
            var (affiliateId, simulationRequest) = request;

            var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(affiliateId, unitOfWork.Transaction);
            if (partnerId.GetValueOrDefault() == Guid.Empty)
                return Result<SimulationWithInstallmentsResponse, Error>.Fail(DomainErrors.Affiliate.NotFound);

            var result = await evaluateCustomerCreditUseCase.ExecuteAsync(partnerId.Value, affiliateId, simulationRequest.CustomerTaxId, unitOfWork.Transaction);
            if (result.TryGetError(out var error))
                return Result<SimulationWithInstallmentsResponse, Error>.Fail(error!);

            if (!result.TryGetSuccess(out var decision))
                return Result<SimulationWithInstallmentsResponse, Error>.Fail(DomainErrors.General.Unexpected);

            if (simulationRequest.RequestedAmount > decision.ApprovedLimit)
                return Result<SimulationWithInstallmentsResponse, Error>.Fail(DomainErrors.Simulation.ExceedsLimit);

            var entity = simulationRequest.ToEntity(
                partnerId: partnerId.Value,
                affiliateId: affiliateId,
                approvedAmount: decision.ApprovedLimit,
                maxInstallments: decision.MaxInstallments,
                interestRate: decision.MonthlyInterestRate,
                user: userContext.GetRequiredUserId()
            );

            await simulationRepository.InsertAsync(entity, unitOfWork.Transaction);

            var installmentOptions = installmentCalculator.Calculate(
                amount: simulationRequest.RequestedAmount,
                maxInstallments: decision.MaxInstallments,
                monthlyInterestRate: decision.MonthlyInterestRate
            );

            var affordableOptions = installmentOptions
                .Where(o => o.Total <= decision.ApprovedLimit)
                .ToList();

            if (affordableOptions.Count == 0)
                return Result<SimulationWithInstallmentsResponse, Error>.Fail(DomainErrors.Simulation.NoInstallmentOption);

            return Result<SimulationWithInstallmentsResponse, Error>.Ok(new SimulationWithInstallmentsResponse
            {
                Simulation = entity.ToResponse(),
                Installments = affordableOptions
            });
        }
    }
}