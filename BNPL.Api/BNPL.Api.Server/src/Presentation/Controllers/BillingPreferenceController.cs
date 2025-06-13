using Core.Persistence.Interfaces;
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
        IUseCase<UpdateCustomerBillingPreferencesRequestUseCase, Result<CustomerBillingPreferencesDto, Error>> updateCustomerBillingPreferencesUseCase
    ) : ControllerBase
    {
        [HttpPut]
        [ProducesResponseType(typeof(Result<CustomerBillingPreferencesDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerBillingPreferencesDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<CustomerBillingPreferencesDto, Error>>> UpdatePreferences(
            [FromQuery] Guid customerId,
            [FromQuery] Guid affiliateId,
            [FromBody] UpdateCustomerBillingPreferencesRequest request)
        {
            var useCaseRequest = new UpdateCustomerBillingPreferencesRequestUseCase(customerId, affiliateId, request);
            var result = await updateCustomerBillingPreferencesUseCase.ExecuteAsync(useCaseRequest);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}