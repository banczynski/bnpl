using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed record CreatePartnerRequestUseCase(CreatePartnerRequest Dto);

    public sealed class CreatePartnerUseCase(
        IPartnerRepository partnerRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreatePartnerRequestUseCase, Result<CreatePartnerResponse, Error>>
    {
        public async Task<Result<CreatePartnerResponse, Error>> ExecuteAsync(CreatePartnerRequestUseCase request)
        {
            var entity = request.Dto.ToEntity(userContext.GetRequiredUserId());
            await partnerRepository.InsertAsync(entity, unitOfWork.Transaction);

            var response = new CreatePartnerResponse(entity.Code);
            return Result<CreatePartnerResponse, Error>.Ok(response);
        }
    }
}