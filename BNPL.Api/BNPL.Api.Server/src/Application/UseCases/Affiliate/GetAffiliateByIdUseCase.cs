using BNPL.Api.Server.src.Application.DTOs.Affiliate;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Affiliate
{
    public sealed class GetAffiliateByIdUseCase(IAffiliateRepository affiliateRepository)
    {
        public async Task<Result<AffiliateDto, Error>> ExecuteAsync(Guid affiliateId)
        {
            var entity = await affiliateRepository.GetByIdAsync(affiliateId);

            if (entity is null)
                return Result<AffiliateDto, Error>.Fail(DomainErrors.Affiliate.NotFound);

            return Result<AffiliateDto, Error>.Ok(entity.ToDto());
        }
    }
}