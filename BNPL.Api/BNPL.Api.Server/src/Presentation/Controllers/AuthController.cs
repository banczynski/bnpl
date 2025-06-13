using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.Auth;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController(ICognitoAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, Error>>> Login([FromBody] LoginRequest request)
        {
            var result = await authService.AuthenticateAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, Error>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await authService.RefreshTokenAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("complete-challenge")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, Error>>> CompleteChallenge([FromBody] CompleteChallengeRequest request)
        {
            var result = await authService.CompleteNewPasswordChallengeAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}