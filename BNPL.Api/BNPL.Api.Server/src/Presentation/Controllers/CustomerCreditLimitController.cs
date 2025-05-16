using BNPL.Api.Server.src.Application.UseCases.CreditLimit;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CustomerCreditLimitController(
        GetCustomerCreditLimitUseCase getUseCase,
        AdjustCustomerCreditLimitUseCase adjustUseCase
    ) : ControllerBase
    {
        [HttpGet("{taxIdd}")]
        public async Task<ActionResult<ServiceResult<object>>> Get(string taxIdd)
            => Ok(await getUseCase.ExecuteAsync(taxIdd));

        [HttpPost("{taxId}/adjust")]
        public async Task<ActionResult<ServiceResult<string>>> Adjust(
            string taxId,
            [FromQuery] decimal value,
            [FromQuery] bool increase = false
        )
        {
            return Ok(await adjustUseCase.ExecuteAsync(taxId, value, increase));
        }
    }
}
