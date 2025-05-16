using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class UpdateCustomerUseCase(
        ICustomerRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CustomerDto>> ExecuteAsync(Guid id, UpdateCustomerRequest request)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Customer not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<CustomerDto>(
                entity.ToDto(),
                ["Customer updated successfully."]
            );
        }
    }
}
