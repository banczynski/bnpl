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
        public async Task<ActionResult<Result<CustomerCreditLimitDto, string>>> Get(
            string taxId,
            [FromQuery] Guid affiliateId)
            => Ok(await getUseCase.ExecuteAsync(taxId, affiliateId));
    }
}
