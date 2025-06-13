using BNPL.Api.Server.src.Application.DTOs.CreditLimit;
using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CustomerCreditLimitController(
        GetCustomerCreditLimitUseCase getUseCase
    ) : ControllerBase
    {
        [HttpGet("{taxId}")]
        [ProducesResponseType(typeof(Result<CustomerCreditLimitDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerCreditLimitDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<CustomerCreditLimitDto, Error>>> Get(
            string taxId,
            [FromQuery] Guid affiliateId)
        {
            var result = await getUseCase.ExecuteAsync(taxId, affiliateId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}