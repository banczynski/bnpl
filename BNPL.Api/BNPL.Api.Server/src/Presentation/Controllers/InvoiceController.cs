using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Invoice;
using BNPL.Api.Server.src.Application.UseCases.Invoice;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InvoiceController(
        IUseCase<CreateInvoiceRequestUseCase, Result<IEnumerable<CreateInvoiceResponse>, Error>> createUseCase,
        IUseCase<GenerateInvoiceBatchRequestUseCase, Result<List<InvoiceDto>, Error>> generateBatchUseCase,
        IUseCase<MarkOverdueInvoicesRequestUseCase, Result<int, Error>> markOverdueUseCase,
        GetInvoiceByIdUseCase getByIdUseCase,
        GetInvoicesByCustomerIdUseCase getByCustomerUseCase,
        GenerateInvoicePaymentLinkUseCase generatePaymentLinkUseCase
    ) : ControllerBase
    {
        [HttpPost("generate/{customerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<CreateInvoiceResponse>, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<CreateInvoiceResponse>, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<IEnumerable<CreateInvoiceResponse>, Error>>> Create(
            Guid customerId,
            [FromQuery] Guid affiliateId)
        {
            var useCaseRequest = new CreateInvoiceRequestUseCase(customerId, affiliateId);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("batch")]
        [ProducesResponseType(typeof(Result<List<InvoiceDto>, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<InvoiceDto>, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<List<InvoiceDto>, Error>>> GenerateBatch([FromQuery] int daysAhead = 0)
        {
            var useCaseRequest = new GenerateInvoiceBatchRequestUseCase(daysAhead);
            var result = await generateBatchUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("mark-overdue")]
        [ProducesResponseType(typeof(Result<int, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<int, Error>>> MarkOverdue()
        {
            var useCaseRequest = new MarkOverdueInvoicesRequestUseCase();
            var result = await markOverdueUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<InvoiceDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<InvoiceDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<InvoiceDto, Error>>> GetById(Guid id)
        {
            var result = await getByIdUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("by-customer/{customerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<InvoiceDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<InvoiceDto>, Error>>> GetByCustomer(Guid customerId)
        {
            var result = await getByCustomerUseCase.ExecuteAsync(customerId);
            return Ok(result);
        }

        [HttpPost("{id:guid}/generate-payment-link")]
        [ProducesResponseType(typeof(Result<GeneratePaymentLinkResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<GeneratePaymentLinkResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<GeneratePaymentLinkResponse, Error>>> GeneratePaymentLink(Guid id)
        {
            var result = await generatePaymentLinkUseCase.ExecuteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}