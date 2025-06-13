using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class UpdateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CustomerDto, string>> ExecuteAsync(Guid customerId, UpdateCustomerRequest request)
        {
            var entity = await customerRepository.GetByIdAsync(customerId);

            if (entity is null)
                return Result<CustomerDto, string>.Fail("Customer not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.GetRequiredUserId());

            await customerRepository.UpdateAsync(entity);

            return Result<CustomerDto, string>.Ok(entity.ToDto());
        }
    }
}
