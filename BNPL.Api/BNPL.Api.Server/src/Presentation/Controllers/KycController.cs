using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.UseCases.Kyc;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class KycController(
        IUseCase<CreateKycRequestUseCase, Result<KycDto, Error>> createUseCase,
        IUseCase<UpdateKycRequestUseCase, Result<KycDto, Error>> updateUseCase,
        GetKycUseCase getUseCase,
        IUseCase<AnalyzeCustomerDocumentRequestUseCase, Result<OcrExtractionResult, Error>> analyzeDocumentUseCase,
        IUseCase<ValidateFaceMatchRequestUseCase, Result<bool, Error>> validateFaceMatchUseCase,
        IUseCase<ValidateKycStatusRequestUseCase, Result<string, Error>> validateKycStatusUseCase
    ) : ControllerBase
    {
        [HttpPost("{customerId:guid}")]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<KycDto, Error>>> Create(Guid customerId, [FromBody] CreateKycRequest request)
        {
            var useCaseRequest = new CreateKycRequestUseCase(customerId, request);
            var result = await createUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{customerId:guid}")]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<KycDto, Error>>> Update(Guid customerId, [FromBody] UpdateKycRequest request)
        {
            var useCaseRequest = new UpdateKycRequestUseCase(customerId, request);
            var result = await updateUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{customerId:guid}")]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<KycDto, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<KycDto, Error>>> Get(Guid customerId)
        {
            var result = await getUseCase.ExecuteAsync(customerId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{customerId:guid}/analyze-document")]
        [ProducesResponseType(typeof(Result<OcrExtractionResult, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OcrExtractionResult, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<OcrExtractionResult, Error>>> Analyze(Guid customerId, [FromBody] AnalyzeCustomerDocumentRequest request)
        {
            var useCaseRequest = new AnalyzeCustomerDocumentRequestUseCase(customerId, request);
            var result = await analyzeDocumentUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{customerId:guid}/validate-face-match")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> ValidateFace(Guid customerId)
        {
            var useCaseRequest = new ValidateFaceMatchRequestUseCase(customerId);
            var result = await validateFaceMatchUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{customerId:guid}/validate-kyc")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> ValidateKycStatus(Guid customerId)
        {
            var useCaseRequest = new ValidateKycStatusRequestUseCase(customerId);
            var result = await validateKycStatusUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}