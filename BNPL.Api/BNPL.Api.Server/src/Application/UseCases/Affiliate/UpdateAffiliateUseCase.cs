using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class UpdateAffiliateUseCase(
        IAffiliateRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<AffiliateDto>> ExecuteAsync(Guid id, UpdateAffiliateRequest request)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Affiliate not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<AffiliateDto>(
                entity.ToDto(),
                ["Affiliate updated successfully."]
            );
        }
    }
}
