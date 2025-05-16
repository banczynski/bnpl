using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.UseCases.Invoice;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InvoiceController(
        CreateInvoiceUseCase createUseCase, 
        GenerateInvoiceBatchUseCase generateBatchUseCase,
        MarkInvoiceAsPaidUseCase markAsPaidUseCase,
        MarkOverdueInvoicesUseCase markOverdueUseCase,
        UpdateInvoiceUseCase updateUseCase,
        InactivateInvoiceUseCase inactivateUseCase,
        GetInvoiceByIdUseCase getByIdUseCase,
        GetInvoicesByCustomerIdUseCase getByCustomerUseCase,
        GenerateInvoicePaymentLinkUseCase generatePaymentLinkUseCase
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResult<CreateInvoiceResponse>>> Create([FromBody] CreateInvoiceRequest request)
        {
            var result = await createUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        // TODO Esse processo deverá ficar numa lambda
        [HttpPost("batch")]
        public async Task<ActionResult<ServiceResult<List<InvoiceDto>>>> GenerateBatch([FromQuery] int daysAhead = 0)
            => Ok(await generateBatchUseCase.ExecuteAsync(daysAhead));

        [HttpPost("{id:guid}/mark-as-paid")]
        public async Task<ActionResult<ServiceResult<string>>> MarkAsPaid(Guid id)
            => Ok(await markAsPaidUseCase.ExecuteAsync(id));

        // TODO Esse processo deverá ficar numa lambda
        [HttpPost("mark-overdue")]
        public async Task<ActionResult<ServiceResult<int>>> MarkOverdue()
            => Ok(await markOverdueUseCase.ExecuteAsync());

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceResult<InvoiceDto>>> Update(Guid id, [FromBody] UpdateInvoiceRequest request)
            => Ok(await updateUseCase.ExecuteAsync(id, request));

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceResult<string>>> Inactivate(Guid id)
            => Ok(await inactivateUseCase.ExecuteAsync(id));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceResult<InvoiceDto>>> GetById(Guid id)
            => Ok(await getByIdUseCase.ExecuteAsync(id));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<InvoiceDto>>>> GetByCustomer(Guid customerId, [FromQuery] bool onlyActive = true)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId, onlyActive));

        [HttpPost("{id:guid}/generate-payment-link")]
        public async Task<ActionResult<ServiceResult<GeneratePaymentLinkResponse>>> GeneratePaymentLink(Guid id)
            => Ok(await generatePaymentLinkUseCase.ExecuteAsync(id));
    }
}
