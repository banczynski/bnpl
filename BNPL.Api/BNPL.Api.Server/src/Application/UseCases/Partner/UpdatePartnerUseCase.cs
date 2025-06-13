using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class UpdatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<PartnerDto, string>> ExecuteAsync(Guid id, UpdatePartnerRequest request)
        {
            var entity = await partnerRepository.GetByIdAsync(id);
            if (entity is null)
                return Result<PartnerDto, string>.Fail("Partner not found.");

            var now = DateTime.UtcNow;
            entity.UpdateEntity(request, now, userContext.GetRequiredUserId());

            await partnerRepository.UpdateAsync(entity);

            return Result<PartnerDto, string>.Ok(entity.ToDto());
        }
    }
}
