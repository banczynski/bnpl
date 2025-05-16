using BNPL.Api.Server.src.Application.DTOs.Payment;
using BNPL.Api.Server.src.Application.UseCases.Payment;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PaymentController(ProcessPaymentCallbackUseCase callbackUseCase) : ControllerBase
    {
        [HttpPost("callback")]
        public async Task<ActionResult<ServiceResult<string>>> ProcessCallback([FromBody] PaymentCallbackRequest request)
            => Ok(await callbackUseCase.ExecuteAsync(request));
    }
}
