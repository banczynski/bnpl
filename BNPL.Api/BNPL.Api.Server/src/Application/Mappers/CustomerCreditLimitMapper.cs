using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Mappers
{
    public static class CustomerCreditLimitMapper
    {
        public static CustomerCreditLimitDto ToDto(this CustomerCreditLimit entity)
        {
            return new CustomerCreditLimitDto(
                Code: entity.Code,
                PartnerId: entity.PartnerId,
                AffiliateId: entity.AffiliateId,
                CustomerTaxId: entity.CustomerTaxId,
                ApprovedLimit: entity.ApprovedLimit,
                UsedLimit: entity.UsedLimit,
                CreatedAt: entity.CreatedAt,
                UpdatedAt: entity.UpdatedAt
            );
        }
    }
}
