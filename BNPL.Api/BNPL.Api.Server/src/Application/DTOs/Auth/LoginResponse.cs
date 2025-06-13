namespace BNPL.Api.Server.src.Application.DTOs.Auth
{
    public sealed class LoginResponse
    {
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public string? IdToken { get; init; }
        public int ExpiresIn { get; init; }
        public bool ChallengeRequired { get; init; }
        public string? ChallengeSession { get; init; }
        public string? ChallengeName { get; init; }
    }
}
