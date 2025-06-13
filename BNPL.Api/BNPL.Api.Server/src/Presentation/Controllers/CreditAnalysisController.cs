using Core.Persistence.Interfaces;
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
        IUseCase<UpdateCreditAnalysisConfigRequestUseCase, Result<CreditAnalysisConfigDto, Error>> updateUseCase,
        IUseCase<InactivateCreditAnalysisConfigRequestUseCase, Result<bool, Error>> inactivateUseCase,
        GetCreditAnalysisConfigByPartnerUseCase getByPartnerUseCase,
        GetCreditAnalysisConfigByAffiliateUseCase getByAffiliateUseCase,
        IUseCase<RunCreditAnalysisRequestUseCase, Result<CreditAnalysisResult, Error>> runCreditAnalysisUseCase
    ) : ControllerBase
    {
        [HttpPut("{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<CreditAnalysisConfigDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CreditAnalysisConfigDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CreditAnalysisConfigDto, Error>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateCreditAnalysisConfigRequest request)
        {
            var useCaseRequest = new UpdateCreditAnalysisConfigRequestUseCase(partnerId, affiliateId, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
        {
            var useCaseRequest = new InactivateCreditAnalysisConfigRequestUseCase(partnerId, affiliateId);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-partner/{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<CreditAnalysisConfigDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CreditAnalysisConfigDto>, Error>>> GetByPartner(Guid partnerId)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId);
            return Ok(result);
        }

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<CreditAnalysisConfigDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CreditAnalysisConfigDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<CreditAnalysisConfigDto, Error>>> GetByAffiliate(Guid affiliateId)
        {
            var result = await getByAffiliateUseCase.ExecuteAsync(affiliateId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("run/{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<PublicCreditAnalysisResult, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PublicCreditAnalysisResult, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<PublicCreditAnalysisResult, Error>>> RunAnalysis(
            Guid partnerId,
            [FromQuery] Guid affiliateId,
            [FromQuery] string customerTaxId)
        {
            var useCaseRequest = new RunCreditAnalysisRequestUseCase(partnerId, affiliateId, customerTaxId);
            var result = await runCreditAnalysisUseCase.ExecuteAsync(useCaseRequest);

            if (result.TryGetError(out var error))
                return BadRequest(Result<PublicCreditAnalysisResult, Error>.Fail(error!));

            if (result.TryGetSuccess(out var decision))
            {
                var publicResult = new PublicCreditAnalysisResult(
                    ApprovedLimit: decision.ApprovedLimit,
                    MaxInstallments: decision.MaxInstallments,
                    MonthlyInterestRate: decision.MonthlyInterestRate
                );
                return Ok(Result<PublicCreditAnalysisResult, Error>.Ok(publicResult));
            }

            return BadRequest(Result<PublicCreditAnalysisResult, Error>.Fail(DomainErrors.General.Unexpected));
        }
    }
}