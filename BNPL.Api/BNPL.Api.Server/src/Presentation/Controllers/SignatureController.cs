using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.UseCases.Signature;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SignatureController(
        IUseCase<GenerateSignatureTokenRequestUseCase, Result<SignatureTokenResponse, Error>> generateSignatureTokenUseCase,
        IUseCase<ConfirmSignatureRequestUseCase, Result<bool, Error>> confirmSignatureUseCase
    ) : ControllerBase
    {
        [HttpPost("generate-signature-token/{proposalId:guid}")]
        [ProducesResponseType(typeof(Result<SignatureTokenResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<SignatureTokenResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<SignatureTokenResponse, Error>>> GenerateSignatureToken(Guid proposalId)
        {
            var useCaseRequest = new GenerateSignatureTokenRequestUseCase(proposalId);
            var result = await generateSignatureTokenUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("confirm-signature/{proposalId:guid}")]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<bool, Error>>> ConfirmSignature(Guid proposalId, [FromQuery] string token)
        {
            var useCaseRequest = new ConfirmSignatureRequestUseCase(proposalId, token);
            var result = await confirmSignatureUseCase.ExecuteAsync(useCaseRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}