using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.User;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(ICognitoUserService service) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> Create([FromBody] CreateUserRequest request)
        {
            var result = await service.CreateUserAsync(request);
            if (result.IsFailure) return BadRequest(result);

            var response = Result<string, Error>.Ok(request.Email);
            return CreatedAtAction(nameof(GetByEmail), new { email = request.Email }, response);
        }

        [HttpDelete("{email}")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> Delete(string email)
        {
            var result = await service.DeleteUserAsync(email);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok(email)) : BadRequest(result);
        }

        [HttpPost("{email}/enable")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> Enable(string email)
        {
            var result = await service.EnableUserAsync(email);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok(email)) : BadRequest(result);
        }

        [HttpPost("{email}/disable")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> Disable(string email)
        {
            var result = await service.DisableUserAsync(email);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok(email)) : BadRequest(result);
        }

        [HttpPost("{email}/reset-password")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> ResetPassword(string email)
        {
            var result = await service.ResetPasswordAsync(email);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok(email)) : BadRequest(result);
        }

        [HttpPost("{email}/groups/{group}")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> AddToGroup(string email, string group)
        {
            var result = await service.AddToGroupAsync(email, group);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok($"{email}:{group}")) : BadRequest(result);
        }

        [HttpDelete("{email}/groups/{group}")]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<string, Error>>> RemoveFromGroup(string email, string group)
        {
            var result = await service.RemoveFromGroupAsync(email, group);
            return result.IsSuccess ? Ok(Result<string, Error>.Ok($"{email}:{group}")) : BadRequest(result);
        }

        [HttpGet("{email}/groups")]
        [ProducesResponseType(typeof(Result<IEnumerable<string>, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IEnumerable<string>, Error>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<IEnumerable<string>, Error>>> GetGroups(string email)
        {
            var result = await service.GetUserGroupsAsync(email);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Result<UserResponse, Error>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<UserResponse, Error>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result<UserResponse, Error>>> GetByEmail(string email)
        {
            var result = await service.GetUserAsync(email);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<UserResponse>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<UserResponse>, Error>>> List()
        {
            var result = await service.ListUsersAsync();
            return Ok(result);
        }

        [HttpGet("groups")]
        [ProducesResponseType(typeof(Result<IEnumerable<string>, Error>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<string>, Error>>> ListGroups()
        {
            var result = await service.ListGroupsAsync();
            return Ok(result);
        }
    }
}