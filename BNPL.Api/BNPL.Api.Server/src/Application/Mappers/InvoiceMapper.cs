using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class InvoiceMapper
    {
        public static Invoice ToEntity(this CreateInvoiceRequest request, Guid id, DateTime now, string user)
            => new()
            {
                Id = id,
                PartnerId = request.PartnerId,
                AffiliateId = request.AffiliateId,
                CustomerId = request.CustomerId,
                CustomerTaxId = request.CustomerTaxId,
                DueDate = request.DueDate,
                TotalAmount = request.TotalAmount,
                Status = InvoiceStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = user,
                UpdatedBy = user,
                IsActive = true
            };

        public static void UpdateEntity(this Invoice entity, UpdateInvoiceRequest request, DateTime now, string user)
        {
            entity.DueDate = request.DueDate;
            entity.TotalAmount = request.TotalAmount;
            entity.UpdatedAt = now;
            entity.UpdatedBy = user;
        }

        public static InvoiceDto ToDto(this Invoice invoice)
            => new(
                invoice.Id,
                invoice.PartnerId,
                invoice.AffiliateId,
                invoice.CustomerId,
                invoice.CustomerTaxId,
                invoice.DueDate,
                invoice.TotalAmount,
                invoice.Status,
                invoice.IsActive,
                invoice.CreatedAt,
                invoice.UpdatedAt,
                invoice.CreatedBy,
                invoice.UpdatedBy
            );
    }
}
