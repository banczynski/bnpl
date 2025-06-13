using Core.Context.Interfaces;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class InactivateCustomerUseCase(
        ICustomerRepository customerRepository,
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid customerId)
        {
            var customer = await customerRepository.GetByIdAsync(customerId);

            if (customer is null)
                return Result<bool, string>.Fail("Customer not found.");

            var hasPendingProposals = await proposalRepository.ExistsActiveByCustomerIdAsync(customerId);

            if (hasPendingProposals)
                return Result<bool, string>.Fail("Customer has active credit proposals and cannot be inactivated.");

            await customerRepository.InactivateAsync(customerId, userContext.GetRequiredUserId(), DateTime.UtcNow);

            return Result<bool, string>.Ok(true);
        }
    }
}
