using BNPL.Api.Server.src.Application.DTOs.CreditAnalysis;
using BNPL.Api.Server.src.Application.UseCases.CreditAnalysis;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CreditAnalysisController(
        UpdateCreditAnalysisConfigUseCase updateUseCase,
        InactivateCreditAnalysisConfigUseCase inactivateUseCase,
        GetCreditAnalysisConfigByPartnerUseCase getByPartnerUseCase,
        GetCreditAnalysisConfigByAffiliateUseCase getByAffiliateUseCase,
        RunCreditAnalysisUseCase runCreditAnalysisUseCase
    ) : ControllerBase
    {
        [HttpPut("{partnerId:guid}")]
        public async Task<ActionResult<Result<CreditAnalysisConfigDto, string>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateCreditAnalysisConfigRequest request)
            => Ok(await updateUseCase.ExecuteAsync(partnerId, affiliateId, request));

        [HttpDelete("{partnerId:guid}")]
        public async Task<ActionResult<Result<bool, string>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
            => Ok(await inactivateUseCase.ExecuteAsync(partnerId, affiliateId));

        [HttpGet("by-partner/{partnerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<CreditAnalysisConfigDto>, string>>> GetByPartner(Guid partnerId)
            => Ok(await getByPartnerUseCase.ExecuteAsync(partnerId));

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        public async Task<ActionResult<Result<CreditAnalysisConfigDto, string>>> GetByAffiliate(Guid affiliateId)
            => Ok(await getByAffiliateUseCase.ExecuteAsync(affiliateId));

        [HttpPost("run/{partnerId:guid}")]
        public async Task<ActionResult<Result<PublicCreditAnalysisResult, string>>> RunAnalysis(
            Guid partnerId,
            [FromQuery] Guid affiliateId,
            [FromQuery] string customerTaxId)
        {
            var result = await runCreditAnalysisUseCase.ExecuteAsync(partnerId, affiliateId, customerTaxId);

            if (result is Result<CreditAnalysisResult, string>.Failure fail)
                return BadRequest(Result<PublicCreditAnalysisResult, string>.Fail(fail.Error));

            if (result is not Result<CreditAnalysisResult, string>.Success success)
                return BadRequest("Unexpected state");

            var decision = success.Value;

            return Ok(Result<PublicCreditAnalysisResult, string>.Ok(new(
                ApprovedLimit: decision.ApprovedLimit,
                MaxInstallments: decision.MaxInstallments,
                MonthlyInterestRate: decision.MonthlyInterestRate
            )));
        }
    }
}
