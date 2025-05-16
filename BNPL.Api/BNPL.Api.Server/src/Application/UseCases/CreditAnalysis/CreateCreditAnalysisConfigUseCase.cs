using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.CreditAnalysis
{
    public sealed class CreateCreditAnalysisConfigUseCase(
        ICreditAnalysisConfigurationRepository repository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(CreateCreditAnalysisConfigRequest request)
        {
            var now = DateTime.UtcNow;
            var entity = request.ToEntity(now, userContext.UserId);
            await repository.InsertAsync(entity);

            return new ServiceResult<string>("Credit analysis configuration created.");
        }
    }
}
