using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class GetRenegotiationByIdUseCase(IRenegotiationRepository repository)
    {
        public async Task<ServiceResult<RenegotiationDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Renegotiation not found.");

            return new ServiceResult<RenegotiationDto>(entity.ToDto());
        }
    }
}
