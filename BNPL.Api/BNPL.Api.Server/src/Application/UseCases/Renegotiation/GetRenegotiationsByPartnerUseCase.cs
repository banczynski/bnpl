using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class GetRenegotiationsByPartnerUseCase(IRenegotiationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<RenegotiationDto>>> ExecuteAsync(Guid partnerId)
        {
            var list = await repository.GetByPartnerIdAsync(partnerId);
            return new ServiceResult<IEnumerable<RenegotiationDto>>(list.Select(r => r.ToDto()));
        }
    }
}
