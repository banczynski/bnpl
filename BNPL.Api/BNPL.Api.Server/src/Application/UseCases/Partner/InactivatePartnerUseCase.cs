using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Partner
{
    public sealed class InactivatePartnerUseCase(
        IPartnerRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid id)
        {
            var now = DateTime.UtcNow;

            await repository.InactivateAsync(id, userContext.UserId, now);

            return new ServiceResult<string>("Partner inactivated successfully.");
        }
    }
}
