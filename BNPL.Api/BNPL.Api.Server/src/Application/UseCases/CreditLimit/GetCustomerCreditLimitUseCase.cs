using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class GetCustomerCreditLimitUseCase(ICustomerCreditLimitRepository customerCreditLimitRepository)
    {
        public async Task<Result<CustomerCreditLimitDto, Error>> ExecuteAsync(string taxId, Guid affiliateId)
        {
            var entity = await customerCreditLimitRepository.GetByTaxIdAndAffiliateIdAsync(taxId, affiliateId);

            if (entity is null)
                return Result<CustomerCreditLimitDto, Error>.Fail(DomainErrors.CreditLimit.NotFound);

            return Result<CustomerCreditLimitDto, Error>.Ok(entity.ToDto());
        }
    }
}