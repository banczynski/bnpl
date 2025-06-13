using BNPL.Api.Server.src.Application.Abstractions.Business;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed record GenerateInstallmentsRequestUseCase(Guid ProposalId);

    public sealed class GenerateInstallmentsUseCase(
        IProposalRepository proposalRepository,
        IInstallmentRepository installmentRepository,
        IInstallmentCalculator installmentCalculator,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<GenerateInstallmentsRequestUseCase, Result<IEnumerable<InstallmentDto>, Error>>
    {
        public async Task<Result<IEnumerable<InstallmentDto>, Error>> ExecuteAsync(GenerateInstallmentsRequestUseCase request)
        {
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<IEnumerable<InstallmentDto>, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != ProposalStatus.Active)
                return Result<IEnumerable<InstallmentDto>, Error>.Fail(DomainErrors.Proposal.MustBeActive);

            var existingInstallments = await installmentRepository.GetByProposalIdAsync(proposal.Code, unitOfWork.Transaction);
            if (existingInstallments.Any(i => i.IsActive))
                return Result<IEnumerable<InstallmentDto>, Error>.Fail(DomainErrors.Installment.AlreadyGenerated);

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
                Code = Guid.NewGuid(),
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

            await installmentRepository.InsertManyAsync(newInstallments, unitOfWork.Transaction);

            return Result<IEnumerable<InstallmentDto>, Error>.Ok(newInstallments.ToDtoList());
        }
    }
}