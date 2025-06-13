using BNPL.Api.Server.src.Application.Abstractions.Business;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record CreateProposalRequestUseCase(Guid SimulationId, int Term);

    public sealed class CreateProposalUseCase(
        IProposalRepository proposalRepository,
        IAffiliateRepository affiliateRepository,
        ISimulationRepository simulationRepository,
        ICustomerRepository customerRepository,
        ICustomerCreditLimitRepository customerCreditLimitRepository,
        IInstallmentCalculator installmentCalculator,
        ICustomerBillingPreferencesRepository customerBillingPreferencesRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateProposalRequestUseCase, Result<CreateProposalResponse, Error>>
    {
        public async Task<Result<CreateProposalResponse, Error>> ExecuteAsync(CreateProposalRequestUseCase request)
        {
            var simulation = await simulationRepository.GetByIdAsync(request.SimulationId, unitOfWork.Transaction);
            if (simulation is null)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.Simulation.NotFound);

            var installmentOptions = installmentCalculator.Calculate(
                amount: simulation.RequestedAmount,
                maxInstallments: simulation.MaxInstallments,
                monthlyInterestRate: simulation.MonthlyInterestRate);

            var selectedOption = installmentOptions.FirstOrDefault(x => x.Term == request.Term);
            if (selectedOption is null)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.Proposal.InvalidInstallmentOption);

            var creditLimit = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(simulation.CustomerTaxId, simulation.AffiliateId, unitOfWork.Transaction);
            if (creditLimit is null)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.CreditLimit.NotFound);

            if (creditLimit.ApprovedLimit - creditLimit.UsedLimit < selectedOption.Total)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.CreditLimit.Insufficient);

            var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(simulation.AffiliateId, unitOfWork.Transaction);
            if (partnerId.GetValueOrDefault() == Guid.Empty)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.Affiliate.NotFound);

            var customerId = await customerRepository.GetIdByTaxIdAsync(simulation.CustomerTaxId, unitOfWork.Transaction);
            if (customerId.GetValueOrDefault() == Guid.Empty)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.Customer.NotFound);

            var preferences = await customerBillingPreferencesRepository.GetByCustomerIdAndAffiliateIdAsync(customerId.Value, simulation.AffiliateId, unitOfWork.Transaction);
            if (preferences is null)
                return Result<CreateProposalResponse, Error>.Fail(DomainErrors.Billing.NotFound);

            var entity = new Domain.Entities.Proposal
            {
                Code = Guid.NewGuid(),
                PartnerId = partnerId.Value,
                AffiliateId = simulation.AffiliateId,
                CustomerId = customerId.Value,
                CustomerTaxId = simulation.CustomerTaxId,
                SimulationId = request.SimulationId,
                RequestedAmount = simulation.RequestedAmount,
                TotalWithCharges = selectedOption.Total,
                Term = request.Term,
                MonthlyInterestRate = simulation.MonthlyInterestRate,
                Status = Domain.Enums.ProposalStatus.Created,
                PreferredDueDay = preferences.InvoiceDueDay,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userContext.GetRequiredUserId(),
                UpdatedBy = userContext.GetRequiredUserId(),
                IsActive = true
            };

            await proposalRepository.InsertAsync(entity, unitOfWork.Transaction);

            return Result<CreateProposalResponse, Error>.Ok(new CreateProposalResponse(entity.Code));
        }
    }
}