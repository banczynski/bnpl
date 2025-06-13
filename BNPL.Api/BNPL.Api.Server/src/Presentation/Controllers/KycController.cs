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
        CreateKycUseCase createUseCase,
        UpdateKycUseCase updateUseCase,
        GetKycUseCase getUseCase,
        AnalyzeCustomerDocumentUseCase analyzeDocumentUseCase,
        ValidateFaceMatchUseCase validateFaceMatchUseCase,
        ValidateKycStatusUseCase validateKycStatusUseCase
    ) : ControllerBase
    {
        [HttpPost("{customerId:guid}")]
        public async Task<ActionResult<Result<KycDto, string>>> Create(Guid customerId, [FromBody] CreateKycRequest request)
            => Ok(await createUseCase.ExecuteAsync(customerId, request));

        [HttpPut("{customerId:guid}")]
        public async Task<ActionResult<Result<KycDto, string>>> Update(Guid customerId, [FromBody] UpdateKycRequest request)
            => Ok(await updateUseCase.ExecuteAsync(customerId, request));

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<Result<KycDto, string>>> Get(Guid customerId)
            => Ok(await getUseCase.ExecuteAsync(customerId));

        [HttpPost("{customerId:guid}/analyze-document")]
        public async Task<ActionResult<Result<OcrExtractionResult, string>>> Analyze(Guid customerId, [FromBody] AnalyzeCustomerDocumentRequest request)
            => Ok(await analyzeDocumentUseCase.ExecuteAsync(customerId, request));

        [HttpPost("{customerId:guid}/validate-face-match")]
        public async Task<ActionResult<Result<bool, string>>> ValidateFace(Guid customerId)
            => Ok(await validateFaceMatchUseCase.ExecuteAsync(customerId));

        // TODO Esse processo deverá ficar numa lambda
        [HttpPost("{customerId:guid}/validate-kyc")]
        public async Task<ActionResult<Result<string, string>>> ValidateKycStatus(Guid customerId)
            => Ok(await validateKycStatusUseCase.ExecuteAsync(customerId));
    }
}
