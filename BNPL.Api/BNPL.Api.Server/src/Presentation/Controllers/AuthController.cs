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
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, string>>> Login([FromBody] LoginRequest request)
        {
            var result = await authService.AuthenticateAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, string>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await authService.RefreshTokenAsync(request);
            return Ok(result);
        }

        [HttpPost("complete-challenge")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<LoginResponse, string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<LoginResponse, string>>> CompleteChallenge([FromBody] CompleteChallengeRequest request)
        {
            var result = await authService.CompleteNewPasswordChallengeAsync(request);
            return Ok(result);
        }
    }
}