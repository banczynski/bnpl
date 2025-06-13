using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed record UpdateAffiliateRequestUseCase(Guid AffiliateId, UpdateAffiliateRequest Dto);

    public sealed class UpdateAffiliateUseCase(
        IAffiliateRepository affiliateRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdateAffiliateRequestUseCase, Result<AffiliateDto, Error>>
    {
        public async Task<Result<AffiliateDto, Error>> ExecuteAsync(UpdateAffiliateRequestUseCase request)
        {
            var entity = await affiliateRepository.GetByIdAsync(request.AffiliateId, unitOfWork.Transaction);
            if (entity is null)
                return Result<AffiliateDto, Error>.Fail(DomainErrors.Affiliate.NotFound);

            var now = DateTime.UtcNow;
            entity.UpdateEntity(request.Dto, now, userContext.GetRequiredUserId());

            await affiliateRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<AffiliateDto, Error>.Ok(entity.ToDto());
        }
    }
}