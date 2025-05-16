using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Infrastructure.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsSignedUseCase(
        IProposalRepository repository,
        IProposalItemRepository proposalItemRepository,
        IInstallmentRepository installmentRepository,
        ICustomerCreditLimitRepository creditLimitRepository,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUserContext userContext)
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await repository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (!proposal.IsActive)
                throw new InvalidOperationException("Inactive proposal cannot be signed.");

            var creditLimit = await creditLimitRepository.GetByTaxIdAsync(proposal.CustomerTaxId)
                ?? throw new InvalidOperationException("Customer credit limit not found.");

            if (creditLimit.ApprovedLimit - creditLimit.UsedLimit < proposal.ApprovedAmount)
                throw new InvalidOperationException("Insufficient credit limit.");

            var items = await proposalItemRepository.GetByProposalIdAsync(proposalId);
            var totalFromItems = items.Sum(x => x.Amount);

            if (totalFromItems != proposal.ApprovedAmount)
                throw new InvalidOperationException("Sum of ProposalItems does not match the approved amount.");

            var now = DateTime.UtcNow;

            proposal.MarkAsSigned(now, userContext.UserId);
            await repository.UpdateAsync(proposal);

            var baseAmount = Math.Floor((proposal.ApprovedAmount / proposal.Installments) * 100) / 100;
            var remainder = proposal.ApprovedAmount - (baseAmount * (proposal.Installments - 1));

            var installments = new List<Domain.Entities.Installment>();

            for (int i = 0; i < proposal.Installments; i++)
            {
                var amount = i == proposal.Installments - 1 ? remainder : baseAmount;
                var dueDate = now.Date.AddDays(30 * (i + 1));

                var installment = InstallmentMapper.ToEntity(
                    id: Guid.NewGuid(),
                    PartnerId: proposal.PartnerId,
                    AffiliateId: proposal.AffiliateId,
                    proposalId: proposal.Id,
                    renegotiationId: null,
                    customerId: proposal.CustomerId,
                    customerTaxId: proposal.CustomerTaxId,
                    sequence: i + 1,
                    dueDate: dueDate,
                    amount: amount,
                    now: now,
                    user: userContext.UserId
                );

                installments.Add(installment);
            }

            await installmentRepository.InsertManyAsync(installments);

            await adjustCreditLimitUseCase.ExecuteAsync(
                proposal.CustomerTaxId,
                proposal.ApprovedAmount,
                increase: false 
            );

            return new ServiceResult<string>("Proposal marked as signed.");
        }
    }
}
