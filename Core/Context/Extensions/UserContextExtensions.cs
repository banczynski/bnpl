using Core.Context.Interfaces;

namespace Core.Context.Extensions
{
    public static class UserContextExtensions
    {
        public static Guid GetRequiredUserId(this IUserContext userContext)
        {
            if (!Guid.TryParse(userContext.UserId?.Value, out var userId))
                throw new UnauthorizedAccessException("Authenticated user ID is invalid or missing.");

            return userId;
        }

        public static string GetRequiredEmail(this IUserContext userContext)
        {
            return userContext.Email?.Value
                ?? throw new UnauthorizedAccessException("Authenticated email is missing.");
        }

        public static string GetRequiredRole(this IUserContext userContext)
        {
            return userContext.Role?.Value
                ?? throw new UnauthorizedAccessException("Authenticated role is missing.");
        }
    }
}
