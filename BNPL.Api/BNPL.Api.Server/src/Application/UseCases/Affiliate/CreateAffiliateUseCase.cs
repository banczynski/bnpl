using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed record CreateAffiliateRequestUseCase(Guid PartnerId, CreateAffiliateRequest Dto);

    public sealed class CreateAffiliateUseCase(
        IAffiliateRepository repository,
        IPartnerRepository partnerRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateAffiliateRequestUseCase, Result<CreateAffiliateResponse, Error>>
    {
        public async Task<Result<CreateAffiliateResponse, Error>> ExecuteAsync(CreateAffiliateRequestUseCase useCaseRequest)
        {
            var (partnerId, request) = useCaseRequest;

            var partnerExists = await partnerRepository.GetByIdAsync(partnerId, unitOfWork.Transaction);
            if (partnerExists is null)
                return Result<CreateAffiliateResponse, Error>.Fail(DomainErrors.Partner.NotFound);

            var entity = request.ToEntity(partnerId, userContext.GetRequiredUserId());
            await repository.InsertAsync(entity, unitOfWork.Transaction);

            var response = new CreateAffiliateResponse(entity.Code);
            return Result<CreateAffiliateResponse, Error>.Ok(response);
        }
    }
}