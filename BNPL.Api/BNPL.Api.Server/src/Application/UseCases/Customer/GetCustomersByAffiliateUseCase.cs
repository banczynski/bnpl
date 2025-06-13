using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomersByAffiliateUseCase(ICustomerRepository customerRepository)
    {
        public async Task<Result<IEnumerable<CustomerDto>, string>> ExecuteAsync(Guid affiliateId, bool onlyActive = true)
        {
            var customers = await customerRepository.GetByAffiliateIdAsync(affiliateId, onlyActive);
            return Result<IEnumerable<CustomerDto>, string>.Ok(customers.Select(c => c.ToDto()));
        }
    }
}
