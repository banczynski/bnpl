using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomersByAffiliateUseCase(ICustomerRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<CustomerDto>>> ExecuteAsync(Guid affiliateId, bool onlyActive = true)
        {
            var customers = await repository.GetByAffiliateIdAsync(affiliateId, onlyActive);
            return new ServiceResult<IEnumerable<CustomerDto>>(customers.Select(c => c.ToDto()));
        }
    }
}
