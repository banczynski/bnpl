using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Presentation.Configurations;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;
using Microsoft.Extensions.Options;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed record ConfirmProposalItemReturnRequestUseCase(Guid ItemId);

    public sealed class ConfirmProposalItemReturnUseCase(
        IProposalItemRepository itemRepository,
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IInstallmentCalculator installmentCalculator,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IOptions<BusinessRulesSettings> businessRules
    ) : IUseCase<ConfirmProposalItemReturnRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(ConfirmProposalItemReturnRequestUseCase request)
        {
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            var item = await itemRepository.GetByIdAsync(request.ItemId, unitOfWork.Transaction);
            if (item is null || !item.Returned || item.ReturnedAt is null)
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.NotEligibleForConfirmation);

            if (item.ReturnConfirmedAt is not null)
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.AlreadyConfirmed);

            var proposal = await proposalRepository.GetByIdAsync(item.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.CreatedAt < now.AddDays(-businessRules.Value.MaxItemReturnDays))
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.ReturnPeriodExpired);

            item.ReturnConfirmedAt = now;
            item.UpdatedAt = now;
            item.UpdatedBy = userId;
            await itemRepository.UpdateAsync(item, unitOfWork.Transaction);

            var activeItems = (await itemRepository.GetByProposalIdAsync(proposal.Code, unitOfWork.Transaction))
                .Where(i => !i.Returned)
                .ToList();

            if (activeItems.Count == 0)
                return Result<bool, Error>.Fail(DomainErrors.ProposalItem.NoActiveItemsRemaining);

            var installments = await installmentRepository.GetByProposalIdAsync(proposal.Code, unitOfWork.Transaction);
            foreach (var inst in installments)
            {
                inst.IsActive = false;
                inst.UpdatedAt = now;
                inst.UpdatedBy = userId;
            }
            await installmentRepository.UpdateManyAsync(installments, unitOfWork.Transaction);

            var newAmount = activeItems.Sum(i => i.Amount);
            var options = installmentCalculator.Calculate(
                amount: newAmount,
                maxInstallments: proposal.Term,
                monthlyInterestRate: proposal.MonthlyInterestRate);

            var selected = options.First(x => x.Term == proposal.Term);
            var delta = proposal.TotalWithCharges - selected.Total;

            if (delta != 0)
            {
                var creditResult = await adjustCreditLimitUseCase.ExecuteAsync(
                    proposal.CustomerTaxId,
                    proposal.AffiliateId,
                    Math.Abs(delta),
                    increase: delta > 0,
                    unitOfWork.Transaction
                );
                if (creditResult.TryGetError(out var error))
                    return Result<bool, Error>.Fail(error!);
            }

            proposal.RequestedAmount = newAmount;
            proposal.TotalWithCharges = selected.Total;
            proposal.MarkAsApproved(now, userId);
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}