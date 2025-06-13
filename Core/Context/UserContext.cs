using Core.Context.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Core.Context
{
    public sealed class UserContext(IHttpContextAccessor accessor) : IUserContext
    {
        public Claim? UserId => accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        public Claim? Email => accessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
        public Claim? Role => accessor.HttpContext?.User?.FindFirst("cognito:groups");
    }
}
