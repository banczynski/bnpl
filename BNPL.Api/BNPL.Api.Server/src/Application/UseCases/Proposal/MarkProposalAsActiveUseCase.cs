using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed record MarkProposalAsActiveRequestUseCase(Guid ProposalId);

    public sealed class MarkProposalAsActiveUseCase(
        IProposalRepository proposalRepository,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<MarkProposalAsActiveRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(MarkProposalAsActiveRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != ProposalStatus.Signed)
                return Result<bool, Error>.Fail(DomainErrors.Proposal.MustBeSigned);

            proposal.MarkAsActive(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal, unitOfWork.Transaction);

            var creditResult = await adjustCreditLimitUseCase.ExecuteAsync(
                proposal.CustomerTaxId,
                proposal.AffiliateId,
                proposal.TotalWithCharges,
                increase: false,
                unitOfWork.Transaction
            );

            if (creditResult.TryGetError(out var error))
                return Result<bool, Error>.Fail(error!);

            return Result<bool, Error>.Ok(true);
        }
    }
}