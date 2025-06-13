using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class CreateProposalUseCase(
        IProposalRepository proposalRepository,
        IAffiliateRepository affiliateRepository,
        ISimulationRepository simulationRepository,
        ICustomerRepository customerRepository,
        ICustomerCreditLimitRepository customerCreditLimitRepository,
        IInstallmentCalculator installmentCalculator,
        ICustomerBillingPreferencesRepository customerBillingPreferencesRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CreateProposalResponse, string>> ExecuteAsync(Guid simulationId, int term)
        {
            var simulation = await simulationRepository.GetByIdAsync(simulationId);
            if (simulation is null)
                return Result<CreateProposalResponse, string>.Fail("Simulation not found.");

            var installmentOptions = installmentCalculator.Calculate(
                amount: simulation.RequestedAmount,
                maxInstallments: simulation.MaxInstallments,
                monthlyInterestRate: simulation.MonthlyInterestRate
            );

            var selectedOption = installmentOptions
                .FirstOrDefault(x => x.Term == term);

            if (selectedOption is null)
                return Result<CreateProposalResponse, string>.Fail("Invalid installment option.");

            var creditLimit = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(simulation.CustomerTaxId, simulation.AffiliateId);
            if (creditLimit is null)
                return Result<CreateProposalResponse, string>.Fail("Customer has no credit limit");

            if (creditLimit.ApprovedLimit - creditLimit.UsedLimit < selectedOption.Total)
                return Result<CreateProposalResponse, string>.Fail("Insufficient available limit");

            var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(simulation.AffiliateId);
            if (partnerId.GetValueOrDefault() == Guid.Empty)
                return Result<CreateProposalResponse, string>.Fail("Affiliate not found.");

            var customerId = await customerRepository.GetIdByTaxIdAsync(simulation.CustomerTaxId);
            if(customerId.GetValueOrDefault() == Guid.Empty)
                return Result<CreateProposalResponse, string>.Fail("Customer not found.");

            var preferences = await customerBillingPreferencesRepository.GetByCustomerIdAndAffiliateIdAsync(customerId.Value, simulation.AffiliateId);
            if (preferences is null)
                return Result<CreateProposalResponse, string>.Fail("Billing preferences not found.");

            var entity = new Domain.Entities.Proposal {
                AffiliateId = simulation.AffiliateId,
                Code = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userContext.GetRequiredUserId(),
                CustomerId = customerId.Value,
                CustomerTaxId = simulation.CustomerTaxId,
                Term = term,
                IsActive = true,
                MonthlyInterestRate = simulation.MonthlyInterestRate,
                PartnerId = partnerId.Value,
                PreferredDueDay = preferences.InvoiceDueDay,
                RequestedAmount = simulation.RequestedAmount,
                SimulationId = simulationId,
                Status = Domain.Enums.ProposalStatus.Created,
                TotalWithCharges = selectedOption.Total,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userContext.GetRequiredUserId()
            };

            await proposalRepository.InsertAsync(entity);

            var response = new CreateProposalResponse(entity.Code);
            return Result<CreateProposalResponse, string>.Ok(response);
        }
    }
}
