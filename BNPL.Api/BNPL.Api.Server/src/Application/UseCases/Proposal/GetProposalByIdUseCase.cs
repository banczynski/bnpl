using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GetProposalByIdUseCase(IProposalRepository proposalRepository)
    {
        public async Task<Result<ProposalDto, string>> ExecuteAsync(Guid id)
        {
            var entity = await proposalRepository.GetByIdAsync(id);
            return entity is null 
                ? Result<ProposalDto, string>.Fail("Proposal not found.") 
                : Result<ProposalDto, string>.Ok(entity.ToDto());
        }
    }
}
