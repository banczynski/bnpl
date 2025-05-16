using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliateByIdUseCase(IAffiliateRepository repository)
    {
        public async Task<ServiceResult<AffiliateDto>> ExecuteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Affiliate not found.");

            return new ServiceResult<AffiliateDto>(entity.ToDto());
        }
    }
}
