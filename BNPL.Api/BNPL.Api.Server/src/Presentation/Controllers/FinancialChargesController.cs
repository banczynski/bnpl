using Core.Persistence.Interfaces;
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
        IUseCase<CreateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>> createUseCase,
        IUseCase<UpdateFinancialChargesConfigRequestUseCase, Result<FinancialChargesConfigDto, Error>> updateUseCase,
        IUseCase<InactivateFinancialChargesConfigRequestUseCase, Result<bool, Error>> inactivateUseCase,
        GetFinancialChargesByPartnerUseCase getByPartnerUseCase,
        GetFinancialChargesByAffiliateUseCase getByAffiliateUseCase
    ) : ControllerBase
    {
        [HttpPost("{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, Error>>> Create(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] CreateFinancialChargesConfigRequest request)
        {
            var useCaseRequest = new CreateFinancialChargesConfigRequestUseCase(partnerId, affiliateId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, Error>>> Update(
            Guid partnerId,
            [FromQuery] Guid? affiliateId,
            [FromBody] UpdateFinancialChargesConfigRequest request)
        {
            var useCaseRequest = new UpdateFinancialChargesConfigRequestUseCase(partnerId, affiliateId, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> Inactivate(Guid partnerId, [FromQuery] Guid? affiliateId)
        {
            var useCaseRequest = new InactivateFinancialChargesConfigRequestUseCase(partnerId, affiliateId);
            var result = await inactivateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-partner/{partnerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<FinancialChargesConfigDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<FinancialChargesConfigDto>, Error>>> GetByPartner(Guid partnerId)
        {
            var result = await getByPartnerUseCase.ExecuteAsync(partnerId);
            return Ok(result);
        }

        [HttpGet("by-affiliate/{affiliateId:guid}")]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<FinancialChargesConfigDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<FinancialChargesConfigDto, Error>>> GetByAffiliate(Guid affiliateId)
        {
            var result = await getByAffiliateUseCase.ExecuteAsync(affiliateId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}