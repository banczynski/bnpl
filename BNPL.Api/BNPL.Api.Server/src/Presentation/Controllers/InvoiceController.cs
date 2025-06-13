using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.UseCases.Invoice;
using BNPL.Api.Server.src.Domain.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InvoiceController(
        CreateInvoiceUseCase createUseCase,
        GenerateInvoiceBatchUseCase generateBatchUseCase,
        MarkOverdueInvoicesUseCase markOverdueUseCase,
        GetInvoiceByIdUseCase getByIdUseCase,
        GetInvoicesByCustomerIdUseCase getByCustomerUseCase,
        GenerateInvoicePaymentLinkUseCase generatePaymentLinkUseCase
    ) : ControllerBase
    {
        [HttpPost("generate/{customerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<CreateInvoiceResponse>, string>>> Create(
            Guid customerId,
            [FromQuery] Guid affiliateId)
            => Ok(await createUseCase.ExecuteAsync(customerId, affiliateId));

        // TODO Esse processo deverá ficar numa lambda
        [HttpPost("batch")]
        public async Task<ActionResult<Result<List<InvoiceDto>, string>>> GenerateBatch([FromQuery] int daysAhead = 0)
            => Ok(await generateBatchUseCase.ExecuteAsync(daysAhead));

        // TODO Esse processo deverá ficar numa lambda
        [HttpPost("mark-overdue")]
        public async Task<ActionResult<Result<int, string>>> MarkOverdue()
            => Ok(await markOverdueUseCase.ExecuteAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<InvoiceDto, string>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<InvoiceDto>, string>>> GetByCustomer(Guid customerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId, onlyActive));

        [HttpPost("{id:guid}/generate-payment-link")]
        public async Task<ActionResult<Result<GeneratePaymentLinkResponse, string>>> GeneratePaymentLink(Guid id)
            => Ok(await generatePaymentLinkUseCase.ExecuteAsync(id));
    }
}
