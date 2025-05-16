using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class CreateAffiliateUseCase(
        IAffiliateRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CreateAffiliateResponse>> ExecuteAsync(CreateAffiliateRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var entity = request.ToEntity(id, now, userContext.UserId);
            await repository.InsertAsync(entity);

            var response = new CreateAffiliateResponse(entity.Id);
            return new ServiceResult<CreateAffiliateResponse>(response, ["Affiliate created successfully."]);
        }
    }
}
