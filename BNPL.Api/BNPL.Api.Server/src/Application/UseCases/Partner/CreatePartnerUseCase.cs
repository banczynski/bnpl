using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Partner;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class CreatePartnerUseCase(
        IPartnerRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<CreatePartnerResponse>> ExecuteAsync(CreatePartnerRequest request)
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var entity = request.ToEntity(id, now, userContext.UserId);
            await repository.InsertAsync(entity);

            var response = new CreatePartnerResponse(entity.Id);
            return new ServiceResult<CreatePartnerResponse>(response, ["Partner created successfully."]);
        }
    }
}
