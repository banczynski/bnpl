using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.DTOs.Signature;
using System.Collections.Concurrent;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    public sealed class FakeSignatureService : ISignatureService
    {
        private static readonly ConcurrentDictionary<Guid, (string Token, string SentTo, DateTime ExpiresAt)> _store = new();

        private const int ExpirationMinutes = 5;

        public Task<SignatureTokenResponse> GenerateSignatureTokenAsync(Guid proposalId, string customerDestination)
        {
            var token = new Random().Next(100_000, 999_999).ToString(); 
            var expiresAt = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

            _store[proposalId] = (token, customerDestination, expiresAt);

            return Task.FromResult(new SignatureTokenResponse(
                customerDestination,
                expiresAt
            ));
        }

        public Task<bool> ValidateSignatureTokenAsync(Guid proposalId, string sentTo)
        {
            if (!_store.TryGetValue(proposalId, out var entry))
                return Task.FromResult(false);

            var (_, expectedDestination, expiresAt) = entry;

            var isValid = string.Equals(expectedDestination, sentTo, StringComparison.OrdinalIgnoreCase)
                          && DateTime.UtcNow <= expiresAt;

            if (isValid)
                _store.TryRemove(proposalId, out _);

            return Task.FromResult(isValid);
        }
    }
}
