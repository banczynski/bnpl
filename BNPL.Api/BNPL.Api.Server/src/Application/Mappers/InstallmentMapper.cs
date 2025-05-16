using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class InstallmentMapper
    {
        public static Installment ToEntity(
            Guid id,
            Guid PartnerId,
            Guid AffiliateId,
            Guid proposalId,
            Guid? renegotiationId,
            Guid customerId,
            string customerTaxId,
            int sequence,
            DateTime dueDate,
            decimal amount,
            DateTime now,
            string user
        ) => new()
        {
            Id = id,
            PartnerId = PartnerId,
            AffiliateId = AffiliateId,
            ProposalId = proposalId,
            CustomerId = customerId,
            RenegotiationId = renegotiationId,
            CustomerTaxId = customerTaxId,
            Sequence = sequence,
            DueDate = dueDate,
            Amount = amount,
            Status = InstallmentStatus.Pending,
            InvoiceId = null,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = user,
            UpdatedBy = user,
            IsActive = true
        };

        public static InstallmentDto ToDto(this Installment i)
            => new(
                i.Id,
                i.PartnerId,
                i.AffiliateId,
                i.ProposalId,
                i.RenegotiationId,
                i.CustomerId,
                i.CustomerTaxId,
                i.Sequence,
                i.DueDate,
                i.Amount,
                i.Status,
                i.InvoiceId
            );

        public static Installment ToEntityFromRenegotiation(
            Guid id,
            Guid PartnerId,
            Guid AffiliateId,
            Guid renegotiationId,
            Guid customerId,
            string customerTaxId,
            int sequence,
            DateTime dueDate,
            decimal amount,
            DateTime now,
            string user
            ) => new()
            {
                Id = id,
                PartnerId = PartnerId,
                AffiliateId = AffiliateId,
                ProposalId = null,
                CustomerId = customerId,
                RenegotiationId = renegotiationId,
                CustomerTaxId = customerTaxId,
                Sequence = sequence,
                DueDate = dueDate,
                Amount = amount,
                Status = InstallmentStatus.Pending,
                InvoiceId = null,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };
    }
}
