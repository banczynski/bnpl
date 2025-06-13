using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class InstallmentMapper
    {
        public static Installment ToEntity(
            Guid partnerId,
            Guid affiliateId,
            Guid? proposalId,
            Guid customerId,
            string customerTaxId,
            int sequence,
            DateTime dueDate,
            decimal amount,
            DateTime now,
            Guid user
        ) => new()
        {
            Code = Guid.NewGuid(),
            PartnerId = partnerId,
            AffiliateId = affiliateId,
            ProposalId = proposalId,
            CustomerId = customerId,
            CustomerTaxId = customerTaxId,
            Sequence = sequence,
            DueDate = dueDate,
            Amount = amount,
            Status = InstallmentStatus.Pending,
            InvoiceId = null,
            PaymentId = null,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = user,
            UpdatedBy = user,
            IsActive = true
        };

        public static InstallmentDto ToDto(this Installment i) => new(
            i.Code,
            i.PartnerId,
            i.AffiliateId,
            i.ProposalId,
            i.CustomerId,
            i.CustomerTaxId,
            i.Sequence,
            i.DueDate,
            i.Amount,
            i.Status,
            i.InvoiceId,
            i.PaymentId
        );

        public static IEnumerable<InstallmentDto> ToDtoList(this IEnumerable<Installment> installments)
            => [.. installments.Select(i => i.ToDto())];
    }
}