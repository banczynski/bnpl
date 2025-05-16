using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class ConfirmRenegotiationUseCase(
        IRenegotiationRepository renegotiationRepository,
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid renegotiationId)
        {
            var renegotiation = await renegotiationRepository.GetByIdAsync(renegotiationId)
                ?? throw new InvalidOperationException("Renegotiation not found.");

            if (renegotiation.Status != RenegotiationStatus.Pending)
                throw new InvalidOperationException("Only pending renegotiations can be confirmed.");

            var now = DateTime.UtcNow;

            var oldInstallments = await installmentRepository.GetPendingByIdsAsync(renegotiation.OriginalInstallmentIds);
            foreach (var old in oldInstallments)
            {
                old.IsActive = false;
                old.UpdatedAt = now;
                old.UpdatedBy = userContext.UserId;
                await installmentRepository.UpdateAsync(old);
            }

            var oldInvoices = await invoiceRepository.GetByIdsAsync(renegotiation.OriginalInvoiceIds);
            foreach (var old in oldInvoices)
            {
                old.IsActive = false;
                old.UpdatedAt = now;
                old.UpdatedBy = userContext.UserId;
                await invoiceRepository.UpdateAsync(old);
            }

            var newInstallments = new List<Domain.Entities.Installment>();
            var baseAmount = Math.Floor((renegotiation.NewTotalAmount / renegotiation.NewInstallments) * 100) / 100;
            var remainder = renegotiation.NewTotalAmount - (baseAmount * (renegotiation.NewInstallments - 1));

            for (int i = 0; i < renegotiation.NewInstallments; i++)
            {
                var amount = i == renegotiation.NewInstallments - 1 ? remainder : baseAmount;
                var dueDate = now.Date.AddDays(30 * (i + 1));

                var installment = InstallmentMapper.ToEntity(
                    id: Guid.NewGuid(),
                    PartnerId: renegotiation.PartnerId,
                    AffiliateId: renegotiation.AffiliateId,
                    proposalId: Guid.Empty,
                    renegotiationId: renegotiation.Id,
                    customerId: renegotiation.CustomerId,
                    customerTaxId: renegotiation.CustomerTaxId,
                    sequence: i + 1,
                    dueDate: dueDate,
                    amount: amount,
                    now: now,
                    user: userContext.UserId
                );

                newInstallments.Add(installment);
            }

            await installmentRepository.InsertManyAsync(newInstallments);

            var newInvoice = new Domain.Entities.Invoice
            {
                Id = Guid.NewGuid(),
                PartnerId = renegotiation.PartnerId,
                AffiliateId = renegotiation.AffiliateId,
                CustomerTaxId = renegotiation.CustomerTaxId,
                DueDate = newInstallments.Min(x => x.DueDate),
                TotalAmount = newInstallments.Sum(x => x.Amount),
                Status = InvoiceStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = userContext.UserId,
                UpdatedBy = userContext.UserId,
                IsActive = true
            };

            await invoiceRepository.InsertAsync(newInvoice);

            foreach (var installment in newInstallments)
            {
                installment.InvoiceId = newInvoice.Id;
                installment.UpdatedAt = now;
                installment.UpdatedBy = userContext.UserId;
                await installmentRepository.UpdateAsync(installment);
            }

            renegotiation.MarkAsConfirmed(now, userContext.UserId);
            await renegotiationRepository.UpdateAsync(renegotiation);

            return new ServiceResult<string>("Renegotiation confirmed, old records cancelled, and new structure generated.");
        }
    }
}
