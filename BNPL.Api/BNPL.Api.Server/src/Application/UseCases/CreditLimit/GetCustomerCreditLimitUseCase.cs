using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class GetCustomerCreditLimitUseCase(ICustomerCreditLimitRepository customerCreditLimitRepository)
    {
        public async Task<Result<CustomerCreditLimitDto, string>> ExecuteAsync(string taxId, Guid affiliateId)
        {
            var entity = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(taxId, affiliateId);

            if (entity is null)
                return Result<CustomerCreditLimitDto, string>.Fail("Customer credit limit not found.");

            return Result<CustomerCreditLimitDto, string>.Ok(entity.ToDto());
        }
    }
}
