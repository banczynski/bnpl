using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomersByPartnerUseCase(ICustomerRepository customerRepository)
    {
        public async Task<Result<IEnumerable<CustomerDto>, string>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var customers = await customerRepository.GetByPartnerIdAsync(partnerId, onlyActive);
            return Result<IEnumerable<CustomerDto>, string>.Ok(customers.Select(c => c.ToDto()));
        }
    }
}
