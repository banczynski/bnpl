using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed record InactivatePartnerRequestUseCase(Guid PartnerId);

    public sealed class InactivatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IAffiliateRepository affiliateRepository,
        IProposalRepository proposalRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<InactivatePartnerRequestUseCase, Result<bool, Error>>
    {
        public async Task<Result<bool, Error>> ExecuteAsync(InactivatePartnerRequestUseCase request)
        {
            var partner = await partnerRepository.GetByIdAsync(request.PartnerId, unitOfWork.Transaction);
            if (partner is null)
                return Result<bool, Error>.Fail(DomainErrors.Partner.NotFound);

            var affiliates = await affiliateRepository.GetActivesByPartnerIdAsync(request.PartnerId, unitOfWork.Transaction);
            if (affiliates.Any())
                return Result<bool, Error>.Fail(DomainErrors.Partner.HasActiveAffiliates);

            var hasActiveProposals = await proposalRepository.ExistsActiveByPartnerIdAsync(request.PartnerId, unitOfWork.Transaction);
            if (hasActiveProposals)
                return Result<bool, Error>.Fail(DomainErrors.Partner.HasActiveProposals);

            await partnerRepository.InactivateAsync(request.PartnerId, userContext.GetRequiredUserId(), unitOfWork.Transaction);

            return Result<bool, Error>.Ok(true);
        }
    }
}