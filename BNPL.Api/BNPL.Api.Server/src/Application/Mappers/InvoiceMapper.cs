using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class InvoiceMapper
    {
        public static InvoiceDto ToDto(this Invoice i)
            => new(
                i.Code,
                i.PartnerId,
                i.AffiliateId,
                i.CustomerId,
                i.CustomerTaxId,
                i.DueDate,
                i.TotalAmount,
                i.Status,
                i.IsIndividual,
                i.IsActive,
                i.CreatedAt,
                i.UpdatedAt,
                i.CreatedBy,
                i.UpdatedBy
            );
    }
}
