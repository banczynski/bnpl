using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class GetCustomerByIdUseCase(ICustomerRepository customerRepository)
    {
        public async Task<Result<CustomerDto, Error>> ExecuteAsync(Guid customerId)
        {
            var entity = await customerRepository.GetByIdAsync(customerId);

            if (entity is null)
                return Result<CustomerDto, Error>.Fail(DomainErrors.Customer.NotFound);

            return Result<CustomerDto, Error>.Ok(entity.ToDto());
        }
    }
}