using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.UseCases.FinancialCharges;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FinancialChargesController(
        CreateFinancialChargesConfigUseCase createUseCase,
        UpdateFinancialChargesConfigUseCase updateUseCase,
        InactivateFinancialChargesConfigUseCase inactivateUseCase,
        GetFinancialChargesByPartnerUseCase getByPartnerUseCase,
        GetFinancialChargesByAffiliateUseCase getByAffiliateUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<string>>> Create([FromBody] CreateFinancialChargesConfigRequest request)
            => Ok(await createUseCase.ExecuteAsync(request));

        [HttpPut("{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateFinancialChargesConfigRequest request)
            => Ok(await updateUseCase.ExecuteAsync(partnerId, affiliateId, request));

        [HttpDelete("{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
            => Ok(await inactivateUseCase.ExecuteAsync(partnerId, affiliateId));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<FinancialChargesConfigDto>>>> GetByPartner(Guid partnerId)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<ServiceResult<FinancialChargesConfigDto>>> GetByAffiliate(Guid affiliateId)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId));
    }
}
