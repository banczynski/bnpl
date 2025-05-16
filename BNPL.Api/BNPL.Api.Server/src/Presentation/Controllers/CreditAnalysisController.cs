using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CreditAnalysisController(
        CreateCreditAnalysisConfigUseCase createUseCase,
        UpdateCreditAnalysisConfigUseCase updateUseCase,
        InactivateCreditAnalysisConfigUseCase inactivateUseCase,
        GetCreditAnalysisConfigByPartnerUseCase getByPartnerUseCase,
        GetCreditAnalysisConfigByAffiliateUseCase getByAffiliateUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<string>>> Create([FromBody] CreateCreditAnalysisConfigRequest request)
            => Ok(await createUseCase.ExecuteAsync(request));

        [HttpPut("{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateCreditAnalysisConfigRequest request)
            => Ok(await updateUseCase.ExecuteAsync(partnerId, affiliateId, request));

        [HttpDelete("{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
            => Ok(await inactivateUseCase.ExecuteAsync(partnerId, affiliateId));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<CreditAnalysisConfigDto>>>> GetByPartner(Guid partnerId)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<ServiceResult<CreditAnalysisConfigDto>>> GetByAffiliate(Guid affiliateId)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId));
    }
}
