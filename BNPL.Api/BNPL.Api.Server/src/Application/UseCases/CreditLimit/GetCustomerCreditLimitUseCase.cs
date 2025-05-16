using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditLimit
{
    public sealed class GetCustomerCreditLimitUseCase(ICustomerCreditLimitRepository repository)
    {
        public async Task<ServiceResult<CustomerCreditLimit>> ExecuteAsync(string taxId)
        {
            var entity = await repository.GetByTaxIdAsync(taxId)
                ?? throw new InvalidOperationException("Customer credit limit not found.");

            return new ServiceResult<CustomerCreditLimit>(entity);
        }
    }
}
