using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class GetPartnerByIdUseCase(IPartnerRepository partnerRepository)
    {
        public async Task<Result<PartnerDto, string>> ExecuteAsync(Guid id)
        {
            var entity = await partnerRepository.GetByIdAsync(id);

            if (entity is null)
                return Result<PartnerDto, string>.Fail("Partner not found.");

            return Result<PartnerDto, string>.Ok(entity.ToDto());
        }
    }
}
