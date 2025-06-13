using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed record UpdatePartnerRequestUseCase(Guid Id, UpdatePartnerRequest Dto);

    public sealed class UpdatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<UpdatePartnerRequestUseCase, Result<PartnerDto, Error>>
    {
        public async Task<Result<PartnerDto, Error>> ExecuteAsync(UpdatePartnerRequestUseCase request)
        {
            var entity = await partnerRepository.GetByIdAsync(request.Id, unitOfWork.Transaction);
            if (entity is null)
                return Result<PartnerDto, Error>.Fail(DomainErrors.Partner.NotFound);

            var now = DateTime.UtcNow;
            entity.UpdateEntity(request.Dto, now, userContext.GetRequiredUserId());

            await partnerRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<PartnerDto, Error>.Ok(entity.ToDto());
        }
    }
}