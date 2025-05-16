using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class GetRenegotiationsByAffiliateUseCase(IRenegotiationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<RenegotiationDto>>> ExecuteAsync(Guid affiliateId)
        {
            var list = await repository.GetByAffiliateIdAsync(affiliateId);
            return new ServiceResult<IEnumerable<RenegotiationDto>>(list.Select(r => r.ToDto()));
        }
    }
}
