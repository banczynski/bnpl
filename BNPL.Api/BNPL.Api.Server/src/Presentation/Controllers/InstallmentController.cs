using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.UseCases.Installment;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class InstallmentController(
        GetInstallmentsByProposalUseCase getByProposalUseCase,
        GetInstallmentsByCustomerUseCase getByCustomerUseCase,
        CalculateInstallmentChargesUseCase calculateChargesUseCase,
        CalculateInstallmentPenaltiesBatchUseCase calculatePenaltiesBatchUseCase
    ) : ControllerBase
    {
        [HttpGet("by-proposal/{proposalId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<InstallmentDto>>>> GetByProposal(Guid proposalId)
            => Ok(await getByProposalUseCase.ExecuteAsync(proposalId));

        [HttpGet("by-customer/{customerId:guid}")]
        public async Task<ActionResult<ServiceResult<IEnumerable<InstallmentDto>>>> GetByCustomer(Guid customerId)
            => Ok(await getByCustomerUseCase.ExecuteAsync(customerId));

        [HttpGet("{id:guid}/charges")]
        public async Task<ActionResult<ServiceResult<InstallmentChargesResult>>> GetCharges(Guid id, [FromQuery] DateTime? paymentDate = null)
            => Ok(await calculateChargesUseCase.ExecuteAsync(id, paymentDate));

        // TODO Esse processo deverá ficar numa lambda
        [HttpGet("penalties")]
        public async Task<ActionResult<ServiceResult<List<InstallmentChargesReportItem>>>> GetBatchPenalties([FromQuery] DateTime? referenceDate = null)
            => Ok(await calculatePenaltiesBatchUseCase.ExecuteAsync(referenceDate));
    }
}
