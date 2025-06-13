using BNPL.Api.Server.src.Application.DTOs.Auth;
using Core.Models;

namespace BNPL.Api.Server.src.Application.Abstractions.Identity
{
    public interface ICognitoAuthService
    {
        Task<Result<LoginResponse, Error>> AuthenticateAsync(LoginRequest request);
        Task<Result<LoginResponse, Error>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<Result<LoginResponse, Error>> CompleteNewPasswordChallengeAsync(CompleteChallengeRequest request);
    }
}
