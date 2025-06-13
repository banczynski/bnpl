using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed record InactivateAffiliateRequestUseCase(Guid AffiliateId);

    public sealed class InactivateAffiliateUseCase(
        IAffiliateRepository affiliateRepository,
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<InactivateAffiliateRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivateAffiliateRequestUseCase request)
        {
            var hasActiveProposals = await proposalRepository.ExistsActiveByAffiliateIdAsync(request.AffiliateId, unitOfWork.Transaction);
            if (hasActiveProposals)
                return Result<bool, Error>.Fail(DomainErrors.Affiliate.HasActiveProposals);

            await affiliateRepository.InactivateAsync(request.AffiliateId, userContext.GetRequiredUserId(), unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}