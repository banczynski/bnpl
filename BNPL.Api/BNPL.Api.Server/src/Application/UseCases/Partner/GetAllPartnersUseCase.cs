using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class GetAllPartnersUseCase(IPartnerRepository partnerRepository)
    {
        public async Task<Result<IEnumerable<PartnerDto>, Error>> ExecuteAsync()
        {
            var partners = await partnerRepository.GetAllAsync();
            var dtos = partners.Select(p => p.ToDto());

            return Result<IEnumerable<PartnerDto>, Error>.Ok(dtos);
        }
    }
}