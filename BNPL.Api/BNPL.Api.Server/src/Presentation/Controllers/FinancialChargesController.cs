using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;
using BNPL.Api.Server.src.Application.UseCases.FinancialCharges;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
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
        [HttpPost("{partnerId:guid}")]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, string>>> Create(
            Guid partnerId,
            [FromQuery] Guid affiliateId,
            [FromBody] CreateFinancialChargesConfigRequest request)
            => Ok(await createUseCase.ExecuteAsync(partnerId, affiliateId, request));

        [HttpPut("{partnerId:guid}")]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, string>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateFinancialChargesConfigRequest request)
            => Ok(await updateUseCase.ExecuteAsync(partnerId, affiliateId, request));

        [HttpDelete("{partnerId:guid}")]
        public async Task<ActionResult<Result<string, string>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
            => Ok(await inactivateUseCase.ExecuteAsync(partnerId, affiliateId));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<FinancialChargesConfigDto>, string>>> GetByPartner(Guid partnerId)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, string>>> GetByAffiliate(Guid affiliateId)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId));
    }
}
