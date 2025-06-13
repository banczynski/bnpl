using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed record UpdateCustomerRequestUseCase(Guid CustomerId, UpdateCustomerRequest Dto);

    public sealed class UpdateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateCustomerRequestUseCase, Result<CustomerDto, Error>>
    {
        public async Task<Result<CustomerDto, Error>> ExecuteAsync(UpdateCustomerRequestUseCase request)
        {
            var entity = await customerRepository.GetByIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (entity is null)
                return Result<CustomerDto, Error>.Fail(DomainErrors.Customer.NotFound);

            var now = DateTime.UtcNow;
            entity.UpdateEntity(request.Dto, now, userContext.GetRequiredUserId());

            await customerRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<CustomerDto, Error>.Ok(entity.ToDto());
        }
    }
}