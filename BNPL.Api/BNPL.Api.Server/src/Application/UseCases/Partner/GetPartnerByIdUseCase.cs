using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class GetPartnerByIdUseCase(IPartnerRepository repository)
    {
        public async Task<ServiceResult<PartnerDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Partner not found.");

            return new ServiceResult<PartnerDto>(entity.ToDto());
        }
    }
}
