using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class MarkProposalAsActiveUseCase(
        IProposalRepository proposalRepository,
        AdjustCustomerCreditLimitUseCase adjustCreditLimitUseCase,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<bool, string>.Fail("Proposal not found.");

            if (proposal.Status != ProposalStatus.Signed)
                return Result<bool, string>.Fail("Proposal must be signed.");

            proposal.MarkAsActive(DateTime.UtcNow, userContext.GetRequiredUserId());
            await proposalRepository.UpdateAsync(proposal);

            await adjustCreditLimitUseCase.ExecuteAsync(
                proposal.CustomerTaxId,
                proposal.AffiliateId,
                proposal.TotalWithCharges,
                increase: false
            );

            return Result<bool, string>.Ok(true);
        }
    }
}
