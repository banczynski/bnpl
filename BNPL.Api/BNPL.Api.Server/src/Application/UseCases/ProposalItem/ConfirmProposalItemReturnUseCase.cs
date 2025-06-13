using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class ConfirmProposalItemReturnUseCase(
        IProposalItemRepository itemRepository,
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IInstallmentCalculator installmentCalculator,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        private const int MaxReturnDays = 7;

        public async Task<Result<bool, string>> ExecuteAsync(Guid itemId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var now = DateTime.UtcNow;
                var userId = userContext.GetRequiredUserId();

                var item = await itemRepository.GetByIdAsync(itemId, scope.Transaction);
                if (item is null || !item.Returned || item.ReturnedAt is null)
                    return Result<bool, string>.Fail("Item not eligible for return confirmation.");

                if (item.ReturnConfirmedAt is not null)
                    return Result<bool, string>.Fail("Item return already confirmed.");

                var proposal = await proposalRepository.GetByIdAsync(item.ProposalId, scope.Transaction);
                if (proposal is null)
                    return Result<bool, string>.Fail("Proposal not found.");

                if (proposal.CreatedAt < now.AddDays(-MaxReturnDays))
                    return Result<bool, string>.Fail("Return period has expired.");

                item.ReturnConfirmedAt = now;
                item.UpdatedAt = now;
                item.UpdatedBy = userId;
                await itemRepository.UpdateAsync(item, scope.Transaction);

                var activeItems = (await itemRepository.GetByProposalIdAsync(proposal.Code, scope.Transaction))
                    .Where(i => !i.Returned)
                    .ToList();

                if (activeItems.Count == 0)
                    return Result<bool, string>.Fail("No active items remaining in the proposal.");

                var installments = await installmentRepository.GetByProposalIdAsync(proposal.Code, scope.Transaction);
                foreach (var inst in installments)
                {
                    inst.IsActive = false;
                    inst.UpdatedAt = now;
                    inst.UpdatedBy = userId;
                }
                await installmentRepository.UpdateManyAsync(installments, scope.Transaction);

                var newAmount = activeItems.Sum(i => i.Amount);

                var options = installmentCalculator.Calculate(
                    amount: newAmount,
                    maxInstallments: proposal.Term,
                    monthlyInterestRate: proposal.MonthlyInterestRate);

                var selected = options.First(x => x.Term == proposal.Term);

                var delta = proposal.TotalWithCharges - selected.Total;
                if (delta > 0)
                {
                    await adjustCreditLimitUseCase.ExecuteAsync(
                        proposal.CustomerTaxId,
                        proposal.AffiliateId,
                        delta,
                        increase: true
                    );
                }
                else if (delta < 0)
                {
                    await adjustCreditLimitUseCase.ExecuteAsync(
                        proposal.CustomerTaxId,
                        proposal.AffiliateId,
                        Math.Abs(delta),
                        increase: false
                    );
                }

                proposal.RequestedAmount = newAmount;
                proposal.TotalWithCharges = selected.Total;
                proposal.UpdatedAt = now;
                proposal.UpdatedBy = userId;
                proposal.MarkAsApproved(now, userId);

                await proposalRepository.UpdateAsync(proposal, scope.Transaction);

                scope.Commit();
                return Result<bool, string>.Ok(true);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
