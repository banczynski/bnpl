using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomersByAffiliateUseCase(ICustomerRepository customerRepository)
    {
        public async Task<Result<IEnumerable<CustomerDto>, Error>> ExecuteAsync(Guid affiliateId)
        {
            var customers = await customerRepository.GetActivesByAffiliateIdAsync(affiliateId);
            return Result<IEnumerable<CustomerDto>, Error>.Ok(customers.Select(c => c.ToDto()));
        }
    }
}