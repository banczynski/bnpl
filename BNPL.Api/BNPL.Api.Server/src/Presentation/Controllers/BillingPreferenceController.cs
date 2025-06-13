using BNPL.Api.Server.src.Application.DTOs.BillingPreferences;
using BNPL.Api.Server.src.Application.UseCases.BillingPreferences;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/billing-preferences")]
    public sealed class BillingPreferencesController(
        UpdateCustomerBillingPreferencesUseCase updateCustomerBillingPreferencesUseCase
    ) : ControllerBase
    {
        [HttpPut]
        [ProducesResponseType(typeof(Result<CustomerBillingPreferencesDto, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerBillingPreferencesDto, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CustomerBillingPreferencesDto, string>>> UpdatePreferences(
            [FromQuery] Guid customerId,
            [FromQuery] Guid affiliateId,
            [FromBody] UpdateCustomerBillingPreferencesRequest request)
        {
            var result = await updateCustomerBillingPreferencesUseCase.ExecuteAsync(customerId, affiliateId, request);
            return Ok(result);
        }
    }
}