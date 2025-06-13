using BNPL.Api.Server.src.Application.DTOs.Auth;
using Core.Models;

namespace BNPL.Api.Server.src.Application.Abstractions.Identity
{
    public interface ICognitoAuthService
    {
        Task<Result<LoginResponse, string>> AuthenticateAsync(LoginRequest request);
        Task<Result<LoginResponse, string>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<Result<LoginResponse, string>> CompleteNewPasswordChallengeAsync(CompleteChallengeRequest request);
    }
}
