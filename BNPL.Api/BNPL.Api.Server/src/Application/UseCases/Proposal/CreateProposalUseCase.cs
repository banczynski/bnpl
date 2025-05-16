using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Proposal;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class CreateProposalUseCase(
        IProposalRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CreateProposalResponse>> ExecuteAsync(CreateProposalRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var entity = request.ToEntity(id, now, userContext.UserId);

            await repository.InsertAsync(entity);

            return new ServiceResult<CreateProposalResponse>(
                new CreateProposalResponse(entity.Id),
                ["Proposal created successfully."]
            );
        }
    }
}
