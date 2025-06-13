using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class UpdateAffiliateUseCase(
        IAffiliateRepository affiliateRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<AffiliateDto, string>> ExecuteAsync(Guid affiliateId, UpdateAffiliateRequest request)
        {
            var entity = await affiliateRepository.GetByIdAsync(affiliateId);

            if (entity is null)
                return Result<AffiliateDto, string>.Fail("Affiliate not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.GetRequiredUserId());

            await affiliateRepository.UpdateAsync(entity);

            return Result<AffiliateDto, string>.Ok(entity.ToDto());
        }
    }
}
