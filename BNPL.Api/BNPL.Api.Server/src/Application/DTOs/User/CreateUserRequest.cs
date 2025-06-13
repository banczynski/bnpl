namespace BNPL.Api.Server.src.Application.DTOs.User
{
    public sealed class CreateUserRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Group { get; init; } 
        public IDictionary<string, string>? Attributes { get; init; }
    }
}
