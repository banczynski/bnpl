using System.Security.Claims;

namespace Core.Context.Interfaces
{
    public interface IUserContext
    {
        Claim? UserId { get; }
        Claim? Email { get; }
        Claim? Role { get; }
    }
}
