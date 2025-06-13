using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class CreatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IUserContext userContext
    )
    {
        public async Task<Result<CreatePartnerResponse, string[]>> ExecuteAsync(CreatePartnerRequest request)
        {
            var entity = request.ToEntity(userContext.GetRequiredUserId());
            await partnerRepository.InsertAsync(entity);

            var response = new CreatePartnerResponse(entity.Code);
            return Result<CreatePartnerResponse, string[]>.Ok(response);
        }
    }
}
