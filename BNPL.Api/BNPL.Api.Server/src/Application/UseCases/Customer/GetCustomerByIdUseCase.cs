using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomerByIdUseCase(ICustomerRepository customerRepository)
    {
        public async Task<Result<CustomerDto, string>> ExecuteAsync(Guid customerId)
        {
            var entity = await customerRepository.GetByIdAsync(customerId);

            if (entity is null)
                return Result<CustomerDto, string>.Fail("Customer not found.");

            return Result<CustomerDto, string>.Ok(entity.ToDto());
        }
    }
}
