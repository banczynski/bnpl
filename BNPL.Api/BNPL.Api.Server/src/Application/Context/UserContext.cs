using BNPL.Api.Server.src.Application.Context.Interfaces;
using System.Security.Claims;

namespace BNPL.Api.Server.src.Application.Context
{
    public sealed class UserContext(IHttpContextAccessor accessor) : IUserContext
    {
        public string UserId
            => accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new InvalidOperationException("UserId not found in token.");
    }
}
