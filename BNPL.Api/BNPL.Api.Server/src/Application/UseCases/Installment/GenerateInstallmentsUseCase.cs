using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class GenerateInstallmentsUseCase(
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IInstallmentCalculator installmentCalculator,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<IEnumerable<InstallmentDto>, string>> ExecuteAsync(Guid proposalId)
        {
            using var scope = unitOfWork;
            scope.Begin();

            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
            if (proposal is null)
                return Result<IEnumerable<InstallmentDto>, string>.Fail("Proposal not found.");

            if (proposal.Status != ProposalStatus.Active)
                return Result<IEnumerable<InstallmentDto>, string>.Fail("Proposal must be active.");

            var existingInstallments = await installmentRepository.GetByProposalIdAsync(proposal.Code, scope.Transaction);
            if (existingInstallments.Any(i => i.IsActive))
                return Result<IEnumerable<InstallmentDto>, string>.Fail("Installments already generated for this proposal.");

            var options = installmentCalculator.Calculate(
                amount: proposal.TotalWithCharges,
                maxInstallments: proposal.Term,
                monthlyInterestRate: proposal.MonthlyInterestRate);

            var selected = options.First(o => o.Term == proposal.Term);

            var previews = installmentCalculator.GenerateInstallments(
                totalAmount: selected.Total,
                quantity: selected.Term,
                preferredDay: proposal.PreferredDueDay,
                referenceDate: now);

            var newInstallments = previews.Select(p => new Domain.Entities.Installment
            {
                PartnerId = proposal.PartnerId,
                AffiliateId = proposal.AffiliateId,
                ProposalId = proposal.Code,
                CustomerId = proposal.CustomerId,
                CustomerTaxId = proposal.CustomerTaxId,
                Sequence = p.Sequence,
                DueDate = p.DueDate,
                Amount = p.Amount,
                Status = InstallmentStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = userId,
                UpdatedBy = userId,
                IsActive = true
            }).ToList();

            await installmentRepository.InsertManyAsync(newInstallments, scope.Transaction);
            scope.Commit();

            return Result<IEnumerable<InstallmentDto>, string>.Ok(newInstallments.ToDtoList());
        }
    }
}
