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
        GenerateInstallmentsUseCase generateInstallmentsUseCase
    ) : ControllerBase
    {
        [HttpPost("generate/{proposalId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, string>>> GenerateInstallments(Guid proposalId)
            => Ok(await generateInstallmentsUseCase.ExecuteAsync(proposalId));

        [HttpGet("by-proposal/{proposalId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, string>>> GetByProposal(Guid proposalId)
            => Ok(await getByProposalUseCase.ExecuteAsync(proposalId));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>, string>>> GetByCustomer(Guid customerId)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId));

        [HttpGet("{id:guid}/charges")]
        public async Task<ActionResult<Result<InstallmentChargesResult, string>>> GetCharges(Guid id, [FromQuery] DateTime? paymentDate = null)
            => Ok(await calculateChargesUseCase.ExecuteAsync(id, paymentDate));

        // TODO Esse processo deverá ficar numa lambda
        [HttpGet("penalties")]
        public async Task<ActionResult<Result<List<InstallmentChargesReportItem>, string>>> GetBatchPenalties([FromQuery] DateTime? referenceDate = null)
            => Ok(await calculatePenaltiesBatchUseCase.ExecuteAsync(referenceDate));
    }
}
