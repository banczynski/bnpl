using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class CreateRenegotiationFromReturnedItemUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IInstallmentRepository installmentRepository,
        IInvoiceRepository invoiceRepository,
        IRenegotiationRepository renegotiationRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            var items = await proposalItemRepository.GetByProposalIdAsync(proposalId);

            var returnedItems = items.Where(x => x.Returned).ToList();
            if (returnedItems.Count == 0)
                throw new InvalidOperationException("No returned items found.");

            var activeItems = items.Where(x => !x.Returned).ToList();
            var originalTotal = items.Sum(x => x.Amount);
            var newTotal = activeItems.Sum(x => x.Amount);

            var installments = await installmentRepository.GetByProposalIdAsync(proposalId);
            var installmentIds = installments.Select(i => i.Id).ToList();

            var invoices = await invoiceRepository.GetByCustomerIdAsync(proposal.CustomerId);
            var invoiceIds = invoices.Select(i => i.Id).ToList();

            var renegotiation = new Domain.Entities.Renegotiation
            {
                Id = Guid.NewGuid(),
                PartnerId = proposal.PartnerId,
                AffiliateId = proposal.AffiliateId,
                CustomerId = proposal.CustomerId,
                CustomerTaxId = proposal.CustomerTaxId,
                OriginalInstallmentIds = installmentIds,
                OriginalInvoiceIds = invoiceIds,
                OriginalTotalAmount = originalTotal,
                NewTotalAmount = newTotal,
                NewInstallments = proposal.Installments,
                MonthlyInterestRate = proposal.MonthlyInterestRate,
                Status = RenegotiationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userContext.UserId,
                UpdatedBy = userContext.UserId,
                IsActive = true
            };

            await renegotiationRepository.InsertAsync(renegotiation);

            return new ServiceResult<string>("Renegotiation created successfully.");
        }
    }
}
