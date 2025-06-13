using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed record CreateProposalItemsRequestUseCase(Guid ProposalId, CreateProposalItemsRequest Dto);

    public sealed class CreateProposalItemsUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateProposalItemsRequestUseCase, Result<IEnumerable<ProposalItemDto>, Error>>
    {
        public async Task<Result<IEnumerable<ProposalItemDto>, Error>> ExecuteAsync(CreateProposalItemsRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<IEnumerable<ProposalItemDto>, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (!proposal.IsActive)
                return Result<IEnumerable<ProposalItemDto>, Error>.Fail(DomainErrors.Proposal.NotActive);

            if (proposal.Status is not ProposalStatus.Created)
                return Result<IEnumerable<ProposalItemDto>, Error>.Fail(DomainErrors.Proposal.NotEligibleForItems);

            var entities = request.Dto.Items.Select(item => item.ToEntity(request.ProposalId, proposal.AffiliateId, userContext.GetRequiredUserId())).ToList();

            await proposalItemRepository.InsertManyAsync(entities, unitOfWork.Transaction);

            return Result<IEnumerable<ProposalItemDto>, Error>.Ok(entities.ToDtoList());
        }
    }
}