using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public record InactivateCustomerRequestUseCase(Guid CustomerId);

    public sealed class InactivateCustomerUseCase(
        ICustomerRepository customerRepository,
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<InactivateCustomerRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivateCustomerRequestUseCase request)
        {
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (customer is null)
                return Result<bool, Error>.Fail(DomainErrors.Customer.NotFound);

            var hasPendingProposals = await proposalRepository.ExistsActiveByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (hasPendingProposals)
                return Result<bool, Error>.Fail(DomainErrors.Customer.HasActiveProposals);

            await customerRepository.InactivateAsync(request.CustomerId, userContext.GetRequiredUserId(), unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}