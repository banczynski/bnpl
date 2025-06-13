using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class InactivatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IAffiliateRepository affiliateRepository,
        IProposalRepository proposalRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<bool, string>> ExecuteAsync(Guid partnerId)
        {
            var partner = await partnerRepository.GetByIdAsync(partnerId);
            if (partner is null)
                return Result<bool, string>.Fail("Partner not found.");

            var affiliates = await affiliateRepository.GetByPartnerIdAsync(partnerId);
            if (affiliates.Any(a => a.IsActive))
                return Result<bool, string>.Fail("Cannot inactivate partner with active affiliates.");

            var hasActiveProposals = await proposalRepository.ExistsActiveByPartnerIdAsync(partnerId);
            if (hasActiveProposals)
                return Result<bool, string>.Fail("Cannot inactivate partner with active or pending proposals.");

            await partnerRepository.InactivateAsync(partnerId, userContext.GetRequiredUserId(), DateTime.UtcNow);

            return Result<bool, string>.Ok(true);
        }
    }
}
