using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InstallmentController(
        GetInstallmentsByProposalUseCase getByProposalUseCase,
        GetInstallmentsByCustomerUseCase getByCustomerUseCase,
        CalculateInstallmentChargesUseCase calculateChargesUseCase,
        CalculateInstallmentPenaltiesBatchUseCase calculatePenaltiesBatchUseCase,
        IUseCase<GenerateInstallmentsRequestUseCase, Result<IEnumerable<InstallmentDto>, Error>> generateInstallmentsUseCase
    ) : ControllerBase
    {
        [HttpPost("generate/{proposalId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<InstallmentDto>, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<InstallmentDto>, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, Error>>> GenerateInstallments(Guid proposalId)
        {
            var useCaseRequest = new GenerateInstallmentsRequestUseCase(proposalId);
            var result = await generateInstallmentsUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-proposal/{proposalId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<InstallmentDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, Error>>> GetByProposal(Guid proposalId)
        {
            var result = await getByProposalUseCase.ExecuteAsync(proposalId);
            return Ok(result);
        }

        [HttpGet("by-customer/{customerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<InstallmentDto>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, Error>>> GetByCustomer(Guid customerId)
        {
            var result = await getByCustomerUseCase.ExecuteAsync(customerId);
            return Ok(result);
        }

        [HttpGet("{id:guid}/charges")]
        [ProducesResponseType(typeof(Result<InstallmentChargesResult, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<InstallmentChargesResult, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<InstallmentChargesResult, Error>>> GetCharges(Guid id, [FromQuery] DateTime? paymentDate = null)
        {
            var result = await calculateChargesUseCase.ExecuteAsync(id, paymentDate);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("penalties")]
        [ProducesResponseType(typeof(Result<List<InstallmentChargesReportItem>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<List<InstallmentChargesReportItem>, Error>>> GetBatchPenalties([FromQuery] DateTime? referenceDate = null)
        {
            var result = await calculatePenaltiesBatchUseCase.ExecuteAsync(referenceDate);
            return Ok(result);
        }
    }
}