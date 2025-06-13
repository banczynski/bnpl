using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Payment
{
    public sealed class PayInvoiceUseCase(
    IInvoiceRepository invoiceRepository,
    IInstallmentRepository installmentRepository,
    IProposalRepository proposalRepository,
    AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
    CalculateInstallmentChargesUseCase calculateChargesUseCase,
    IUnitOfWork unitOfWork,
    IUserContext userContext
)
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid invoiceId, decimal paidAmount, DateTime? paymentDate = null)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var now = DateTime.UtcNow;
                var userId = userContext.GetRequiredUserId();

                var invoice = await invoiceRepository.GetByIdAsync(invoiceId, scope.Transaction);
                if (invoice is null)
                    return Result<bool, string>.Fail("Invoice not found.");

                if (invoice.Status == InvoiceStatus.Paid)
                    return Result<bool, string>.Fail("Invoice already paid.");

                var installments = await installmentRepository.GetByInvoiceIdAsync(invoiceId, scope.Transaction);
                if (installments.Any(i => i.Status == InstallmentStatus.Paid))
                    return Result<bool, string>.Fail("Some installments already paid.");

                var proposalIds = installments
                    .Where(i => i.ProposalId.HasValue)
                    .Select(i => i.ProposalId!.Value)
                    .Distinct()
                    .ToList();

                foreach (var proposalId in proposalIds)
                {
                    var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                    if (proposal is null)
                        return Result<bool, string>.Fail("Associated proposal not found.");

                    if (proposal.Status != ProposalStatus.Active)
                        return Result<bool, string>.Fail("Payments are only allowed for active proposals.");
                }

                decimal totalDue = 0m;
                foreach (var installment in installments)
                {
                    var chargesResult = await calculateChargesUseCase.ExecuteAsync(installment.Code, paymentDate);
                    if (chargesResult is Result<InstallmentChargesResult, string>.Failure fail)
                        return Result<bool, string>.Fail(fail.Error);

                    totalDue += chargesResult.TryGetSuccess(out var value) ? value.TotalWithCharges : 0m;
                }

                if (paidAmount < totalDue)
                    return Result<bool, string>.Fail("Paid amount is less than total due with penalties.");

                invoice.MarkAsPaid(now, userId);
                await invoiceRepository.UpdateAsync(invoice, scope.Transaction);

                foreach (var installment in installments)
                {
                    installment.MarkAsPaid(now, userId);
                }
                await installmentRepository.UpdateManyAsync(installments, scope.Transaction);

                foreach (var proposalId in proposalIds)
                {
                    var all = await installmentRepository.GetByProposalIdAsync(proposalId, scope.Transaction);
                    if (all.All(i => i.Status == InstallmentStatus.Paid))
                    {
                        var proposal = await proposalRepository.GetByIdAsync(proposalId, scope.Transaction);
                        if (proposal is not null && proposal.Status != ProposalStatus.Finalized)
                        {
                            proposal.MarkAsFinalized(now, userId);
                            await proposalRepository.UpdateAsync(proposal, scope.Transaction);
                        }
                    }
                }

                await adjustCreditLimitUseCase.ExecuteAsync(
                    invoice.CustomerTaxId,
                    invoice.AffiliateId,
                    invoice.TotalAmount,
                    increase: true
                );

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