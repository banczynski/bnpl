using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class GetAllPartnersUseCase(IPartnerRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<PartnerDto>>> ExecuteAsync(bool onlyActive = true)
        {
            var partners = await repository.GetAllAsync(onlyActive);
            return new ServiceResult<IEnumerable<PartnerDto>>(partners.Select(p => p.ToDto()));
        }
    }
}
