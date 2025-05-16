using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class UpdateProposalUseCase(
        IProposalRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<ProposalDto>> ExecuteAsync(Guid id, UpdateProposalRequest request)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Proposal not found.");

            var now = DateTime.UtcNow;

            entity.UpdateEntity(request, now, userContext.UserId);

            await repository.UpdateAsync(entity);

            return new ServiceResult<ProposalDto>(
                entity.ToDto(),
                ["Proposal updated successfully."]
            );
        }
    }
}
