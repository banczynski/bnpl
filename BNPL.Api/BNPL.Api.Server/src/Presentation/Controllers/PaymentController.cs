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
        PayInvoiceUseCase payInvoiceUseCase) : ControllerBase
    {
        [HttpPost("pay-invoice/{invoiceId:guid}")]
        public async Task<ActionResult<Result<string, string[]>>> PayInvoice(
            Guid invoiceId,
            [FromQuery] decimal paidAmount,
            [FromQuery] DateTime? paymentDate)
        {
            var result = await payInvoiceUseCase.ExecuteAsync(invoiceId, paidAmount, paymentDate);
            return Ok(result);
        }
    }
}
