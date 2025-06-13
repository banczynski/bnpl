using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class InactivateAffiliateUseCase(
        IAffiliateRepository affiliateRepository,
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid affiliateId)
        {
            var hasActiveProposals = await proposalRepository.ExistsActiveByAffiliateIdAsync(affiliateId);
            if (hasActiveProposals)
                return Result<bool, string>.Fail("Unable to inactivate affiliate with active proposals.");

            await affiliateRepository.InactivateAsync(affiliateId, userContext.GetRequiredUserId());

            return Result<bool, string>.Ok(true);
        }
    }
}