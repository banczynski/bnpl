using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomerByIdUseCase(ICustomerRepository repository)
    {
        public async Task<ServiceResult<CustomerDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Customer not found.");

            return new ServiceResult<CustomerDto>(entity.ToDto());
        }
    }
}
