namespace BNPL.Api.Server.src.Application.DTOs.Auth
{
    public sealed class CompleteChallengeRequest
    {
        public required string Email { get; init; }
        public required string NewPassword { get; init; }
        public required string Session { get; init; }
    }
}
