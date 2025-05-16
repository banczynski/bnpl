using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.FinancialCharges
{
    public sealed class CreateFinancialChargesConfigUseCase(
        IFinancialChargesConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(CreateFinancialChargesConfigRequest request)
        {
            var now = DateTime.UtcNow;
            var entity = request.ToEntity(now, userContext.UserId);
            await repository.InsertAsync(entity);

            return new ServiceResult<string>("Financial charges configuration created.");
        }
    }
}
