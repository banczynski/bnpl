using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.ProposalItem;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed record CreateProposalItemRequestUseCase(Guid ProposalId, CreateProposalItemRequest Dto);

    public sealed class CreateProposalItemUseCase(
        IProposalRepository proposalRepository,
        IProposalItemRepository proposalItemRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateProposalItemRequestUseCase, Result<ProposalItemDto, Error>>
    {
        public async Task<Result<ProposalItemDto, Error>> ExecuteAsync(CreateProposalItemRequestUseCase request)
        {
            var proposal = await proposalRepository.GetByIdAsync(request.ProposalId, unitOfWork.Transaction);
            if (proposal is null)
                return Result<ProposalItemDto, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (!proposal.IsActive)
                return Result<ProposalItemDto, Error>.Fail(DomainErrors.Proposal.NotActive);

            if (proposal.Status is not ProposalStatus.Created)
                return Result<ProposalItemDto, Error>.Fail(DomainErrors.Proposal.NotEligibleForItems);

            var entity = request.Dto.ToEntity(request.ProposalId, proposal.AffiliateId, userContext.GetRequiredUserId());

            await proposalItemRepository.InsertAsync(entity, unitOfWork.Transaction);

            return Result<ProposalItemDto, Error>.Ok(entity.ToDto());
        }
    }
}