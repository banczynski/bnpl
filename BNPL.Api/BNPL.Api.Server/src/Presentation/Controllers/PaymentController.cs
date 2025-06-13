using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.UseCases.Payment;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PaymentController(
        IUseCase<PayInvoiceRequestUseCase, Result<bool, Error>> payInvoiceUseCase
    ) : ControllerBase
    {
        [HttpPost("pay-invoice/{invoiceId:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> PayInvoice(
            Guid invoiceId,
            [FromQuery] decimal paidAmount,
            [FromQuery] DateTime? paymentDate)
        {
            var useCaseRequest = new PayInvoiceRequestUseCase(invoiceId, paidAmount, paymentDate);
            var result = await payInvoiceUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}