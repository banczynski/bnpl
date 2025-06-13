using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Payment
{
    public sealed record PayInvoiceRequestUseCase(Guid InvoiceId, decimal PaidAmount, DateTime? PaymentDate = null);

    public sealed class PayInvoiceUseCase(
        IInvoiceRepository invoiceRepository,
        IInstallmentRepository installmentRepository,
        IProposalRepository proposalRepository,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        CalculateInstallmentChargesUseCase calculateChargesUseCase,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<PayInvoiceRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(PayInvoiceRequestUseCase request)
        {
            var (invoiceId, paidAmount, paymentDate) = request;
            var now = DateTime.UtcNow;
            var userId = userContext.GetRequiredUserId();

            var invoice = await invoiceRepository.GetByIdAsync(invoiceId, unitOfWork.Transaction);
            if (invoice is null)
                return Result<bool, Error>.Fail(DomainErrors.Invoice.NotFound);

            if (invoice.Status == InvoiceStatus.Paid)
                return Result<bool, Error>.Fail(DomainErrors.Invoice.AlreadyPaid);

            var installments = (await installmentRepository.GetByInvoiceIdAsync(invoiceId, unitOfWork.Transaction)).ToList();
            if (installments.Any(i => i.Status == InstallmentStatus.Paid))
                return Result<bool, Error>.Fail(DomainErrors.Installment.AlreadyPaid);

            var proposalIds = installments
                .Where(i => i.ProposalId.HasValue)
                .Select(i => i.ProposalId!.Value)
                .Distinct()
                .ToList();

            foreach (var proposalId in proposalIds)
            {
                var proposal = await proposalRepository.GetByIdAsync(proposalId, unitOfWork.Transaction);
                if (proposal is null)
                    return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);
                if (proposal.Status != ProposalStatus.Active)
                    return Result<bool, Error>.Fail(DomainErrors.Proposal.PaymentsNotAllowed);
            }

            decimal totalDue = 0m;
            foreach (var installment in installments)
            {
                var chargesResult = await calculateChargesUseCase.ExecuteAsync(installment.Code, paymentDate);
                if (chargesResult.TryGetError(out var error))
                    return Result<bool, Error>.Fail(error!);

                totalDue += chargesResult.TryGetSuccess(out var value) ? value.TotalWithCharges : 0m;
            }

            if (paidAmount < totalDue)
                return Result<bool, Error>.Fail(DomainErrors.Payment.AmountLessThanDue);

            invoice.MarkAsPaid(now, userId);
            await invoiceRepository.UpdateAsync(invoice, unitOfWork.Transaction);

            foreach (var installment in installments)
            {
                installment.MarkAsPaid(now, userId);
            }
            await installmentRepository.UpdateManyAsync(installments, unitOfWork.Transaction);

            foreach (var proposalId in proposalIds)
            {
                var all = await installmentRepository.GetByProposalIdAsync(proposalId, unitOfWork.Transaction);
                if (all.All(i => i.Status == InstallmentStatus.Paid))
                {
                    var proposal = await proposalRepository.GetByIdAsync(proposalId, unitOfWork.Transaction);
                    if (proposal is not null && proposal.Status != ProposalStatus.Finalized)
                    {
                        proposal.MarkAsFinalized(now, userId);
                        await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);
                    }
                }
            }

            var creditLimitResult = await adjustCreditLimitUseCase.ExecuteAsync(
                invoice.CustomerTaxId,
                invoice.AffiliateId,
                invoice.TotalAmount,
                increase: true,
                unitOfWork.Transaction
            );

            if (creditLimitResult.TryGetError(out var creditError))
                return Result<bool, Error>.Fail(creditError!);

            return Result<bool, Error>.Ok(true);
        }
    }
}