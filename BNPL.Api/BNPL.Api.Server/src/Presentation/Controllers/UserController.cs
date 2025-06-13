using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.User;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(ICognitoUserService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result<string, string[]>>> Create([FromBody] CreateUserRequest request)
        {
            await service.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetByEmail), new { email = request.Email }, Result<string, string[]>.Ok(request.Email));
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult<Result<string, string[]>>> Delete(string email)
        {
            await service.DeleteUserAsync(email);
            return Ok(Result<string, string[]>.Ok(email));
        }

        [HttpPost("{email}/enable")]
        public async Task<ActionResult<Result<string, string[]>>> Enable(string email)
        {
            await service.EnableUserAsync(email);
            return Ok(Result<string, string[]>.Ok(email));
        }

        [HttpPost("{email}/disable")]
        public async Task<ActionResult<Result<string, string[]>>> Disable(string email)
        {
            await service.DisableUserAsync(email);
            return Ok(Result<string, string[]>.Ok(email));
        }

        [HttpPost("{email}/reset-password")]
        public async Task<ActionResult<Result<string, string[]>>> ResetPassword(string email)
        {
            await service.ResetPasswordAsync(email);
            return Ok(Result<string, string[]>.Ok(email));
        }

        [HttpPost("{email}/groups/{group}")]
        public async Task<ActionResult<Result<string, string[]>>> AddToGroup(string email, string group)
        {
            await service.AddToGroupAsync(email, group);
            return Ok(Result<string, string[]>.Ok($"{email}:{group}"));
        }

        [HttpDelete("{email}/groups/{group}")]
        public async Task<ActionResult<Result<string, string[]>>> RemoveFromGroup(string email, string group)
        {
            await service.RemoveFromGroupAsync(email, group);
            return Ok(Result<string, string[]>.Ok($"{email}:{group}"));
        }

        [HttpGet("{email}/groups")]
        public async Task<ActionResult<Result<IEnumerable<string>, string[]>>> GetGroups(string email)
        {
            var groups = await service.GetUserGroupsAsync(email);
            return Ok(Result<IEnumerable<string>, string[]>.Ok(groups));
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Result<UserResponse, string[]>>> GetByEmail(string email)
        {
            var user = await service.GetUserAsync(email);
            return Ok(Result<UserResponse, string[]>.Ok(user));
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<UserResponse>, string[]>>> List()
        {
            var users = await service.ListUsersAsync();
            return Ok(Result<IEnumerable<UserResponse>, string[]>.Ok(users));
        }

        [HttpGet("groups")]
        public async Task<ActionResult<Result<IEnumerable<string>, string[]>>> ListGroups()
        {
            var groups = await service.ListGroupsAsync();
            return Ok(Result<IEnumerable<string>, string[]>.Ok(groups));
        }
    }
}
