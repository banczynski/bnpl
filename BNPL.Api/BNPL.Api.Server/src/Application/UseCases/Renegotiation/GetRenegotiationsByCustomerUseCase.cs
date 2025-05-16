using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class GetRenegotiationsByCustomerUseCase(IRenegotiationRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<RenegotiationDto>>> ExecuteAsync(Guid customerId)
        {
            var items = await repository.GetByCustomerIdAsync(customerId);
            return new ServiceResult<IEnumerable<RenegotiationDto>>(items.Select(r => r.ToDto()));
        }
    }
}
