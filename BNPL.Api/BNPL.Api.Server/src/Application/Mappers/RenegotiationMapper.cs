using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class RenegotiationMapper
    {
        public static Renegotiation ToEntity(this CreateRenegotiationRequest request, Guid id, DateTime now, string user, List<Guid> invoiceIds, decimal originalTotal)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                CustomerId = request.CustomerId,
                CustomerTaxId = request.CustomerTaxId,
                OriginalInvoiceIds = invoiceIds,
                OriginalInstallmentIds = request.InstallmentIds,
                OriginalTotalAmount = originalTotal,
                NewTotalAmount = request.NewTotalAmount,
                NewInstallments = request.NewInstallments,
                MonthlyInterestRate = request.MonthlyInterestRate,
                Status = RenegotiationStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static RenegotiationDto ToDto(this Renegotiation r)
            => new(
                r.Id,
                r.PartnerId,
                r.AffiliateId,
                r.CustomerId,
                r.CustomerTaxId,
                r.OriginalInvoiceIds,
                r.OriginalInstallmentIds,
                r.OriginalTotalAmount,
                r.NewTotalAmount,
                r.NewInstallments,
                r.MonthlyInterestRate,
                r.Status,
                r.IsActive,
                r.CreatedAt,
                r.UpdatedAt,
                r.CreatedBy,
                r.UpdatedBy
            );
    }
}
