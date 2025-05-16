using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomersByPartnerUseCase(ICustomerRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<CustomerDto>>> ExecuteAsync(Guid partnerId, bool onlyActive = true)
        {
            var customers = await repository.GetByPartnerIdAsync(partnerId, onlyActive);
            return new ServiceResult<IEnumerable<CustomerDto>>(customers.Select(c => c.ToDto()));
        }
    }
}
