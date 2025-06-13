using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class GetPartnerByIdUseCase(IPartnerRepository partnerRepository)
    {
        public async Task<Result<PartnerDto, Error>> ExecuteAsync(Guid id)
        {
            var entity = await partnerRepository.GetByIdAsync(id);

            if (entity is null)
                return Result<PartnerDto, Error>.Fail(DomainErrors.Partner.NotFound);

            return Result<PartnerDto, Error>.Ok(entity.ToDto());
        }
    }
}