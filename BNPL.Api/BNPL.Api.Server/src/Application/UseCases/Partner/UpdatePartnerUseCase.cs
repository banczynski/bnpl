using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class UpdatePartnerUseCase(
        IPartnerRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<PartnerDto>> ExecuteAsync(Guid id, UpdatePartnerRequest request)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Partner not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<PartnerDto>(
                entity.ToDto(),
                ["Partner updated successfully."]
            );
        }
    }
}
