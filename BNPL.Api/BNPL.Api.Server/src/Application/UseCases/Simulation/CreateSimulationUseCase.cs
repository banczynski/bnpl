using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.DTOs.Simulation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Simulation
{
    public sealed class CreateSimulationUseCase(
        ISimulationRepository repository,
        ICreditAnalysisService creditAnalysisService,
        UpsertCustomerCreditLimitUseCase creditLimitUseCase,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<SimulationResponse>> ExecuteAsync(CreateSimulationRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            // TODO
            var decision = await creditAnalysisService.AnalyzeAsync(request.PartnerId, request.AffiliateId, request.CustomerTaxId, request.RequestedAmount);

            if ((decision.Decision != Domain.Enums.CreditAnalysisStatus.Approved) || decision.ApprovedAmount <= 0)
                throw new InvalidOperationException("Credit denied.");

            await creditLimitUseCase.ExecuteAsync(new CreditLimitUpsertRequest(
                PartnerId: request.PartnerId,
                AffiliateId: request.AffiliateId,
                CustomerTaxId: request.CustomerTaxId,
                ApprovedLimit: decision.ApprovedAmount
            ));

            var entity = request.ToEntity(
                approvedAmount: decision.ApprovedAmount,
                maxInstallments: decision.MaxInstallments,
                interestRate: decision.MonthlyInterestRate,
                id: id,
                now: now,
                user: userContext.UserId
            );

            await repository.InsertAsync(entity);

            return new ServiceResult<SimulationResponse>(
                entity.ToResponse(),
                ["Simulation completed successfully."]
            );
        }
    }
}
