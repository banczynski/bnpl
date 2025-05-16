using BNPL.Api.Server.src.Application.DTOs.Signature;
using BNPL.Api.Server.src.Application.UseCases.Signature;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class SignatureController(ProcessSignatureCallbackUseCase callbackUseCase) : ControllerBase
    {
        [HttpPost("callback")]
        public async Task<ActionResult<ServiceResult<string>>> ProcessCallback([FromBody] SignatureCallbackRequest request)
            => Ok(await callbackUseCase.ExecuteAsync(request.ExternalSignatureId));
    }
}
