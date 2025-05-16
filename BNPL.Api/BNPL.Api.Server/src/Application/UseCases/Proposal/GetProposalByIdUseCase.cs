using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalByIdUseCase(IProposalRepository repository)
    {
        public async Task<ServiceResult<ProposalDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Proposal not found.");

            return new ServiceResult<ProposalDto>(entity.ToDto());
        }
    }
}
