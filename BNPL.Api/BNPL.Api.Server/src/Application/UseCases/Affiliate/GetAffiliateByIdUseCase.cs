using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliateByIdUseCase(IAffiliateRepository affiliateRepository)
    {
        public async Task<Result<AffiliateDto, string>> ExecuteAsync(Guid affiliateId)
        {
            var entity = await affiliateRepository.GetByIdAsync(affiliateId);

            if (entity is null)
                return Result<AffiliateDto, string>.Fail("Affiliate not found.");

            return Result<AffiliateDto, string>.Ok(entity.ToDto());
        }
    }
}
