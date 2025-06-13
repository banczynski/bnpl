namespace BNPL.Api.Server.src.Application.DTOs.User
{
    public sealed class UserResponse
    {
        public required string Email { get; init; }
        public required bool Enabled { get; init; }
        public required bool Confirmed { get; init; }
        public IReadOnlyDictionary<string, string> Attributes { get; init; } = new Dictionary<string, string>();
        public IReadOnlyCollection<string> Groups { get; init; } = [];
    }

}
