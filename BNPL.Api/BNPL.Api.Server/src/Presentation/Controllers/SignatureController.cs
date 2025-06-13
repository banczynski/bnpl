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
        GenerateSignatureTokenUseCase generateSignatureTokenUseCase,
        ConfirmSignatureUseCase confirmSignatureUseCase
    ) : ControllerBase
    {
        [HttpPost("generate-signature-token/{proposalId:guid}")]
        public async Task<ActionResult<Result<SignatureTokenResponse, string>>> GenerateSignatureToken(Guid proposalId)
            => Ok(await generateSignatureTokenUseCase.ExecuteAsync(proposalId));

        [HttpPost("confirm-signature/{proposalId:guid}")]
        public async Task<ActionResult<Result<string, string>>> ConfirmSignature(Guid proposalId, [FromQuery] string token)
            => Ok(await confirmSignatureUseCase.ExecuteAsync(proposalId, token));
    }
}
