using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class CreateAffiliateUseCase(
        IAffiliateRepository repository,
        IPartnerRepository partnerRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CreateAffiliateResponse, string>> ExecuteAsync(Guid partnerId, CreateAffiliateRequest request)
        {
            var partnerExists = await partnerRepository.GetByIdAsync(partnerId);
            if (partnerExists is null)
                return Result<CreateAffiliateResponse, string>.Fail("Partner not found.");

            var entity = request.ToEntity(partnerId, userContext.GetRequiredUserId());
            await repository.InsertAsync(entity);

            var response = new CreateAffiliateResponse(entity.Code);
            return Result<CreateAffiliateResponse, string>.Ok(response);
        }
    }
}