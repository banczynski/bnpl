using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.Auth;
using Core.Models;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class CognitoAuthService(
        IAmazonCognitoIdentityProvider client,
        IConfiguration config
    ) : ICognitoAuthService
    {
        private readonly string clientId = config["AWS:Cognito:ClientId"]
            ?? throw new InvalidOperationException("Cognito ClientId not configured.");

        public async Task<Result<LoginResponse, string>> AuthenticateAsync(LoginRequest request)
        {
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", request.Email },
                    { "PASSWORD", request.Password }
                }
            };

            try
            {
                var response = await client.InitiateAuthAsync(authRequest);

                if (response.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    return Result<LoginResponse, string>.Ok(new LoginResponse
                    {
                        ChallengeRequired = true,
                        ChallengeName = response.ChallengeName.Value,
                        ChallengeSession = response.Session
                    });
                }

                var auth = response.AuthenticationResult;
                if (auth is null)
                    return Result<LoginResponse, string>.Fail("Authentication failed: no token returned.");

                return Result<LoginResponse, string>.Ok(new LoginResponse
                {
                    AccessToken = auth.AccessToken,
                    RefreshToken = auth.RefreshToken,
                    IdToken = auth.IdToken,
                    ExpiresIn = auth.ExpiresIn.GetValueOrDefault()
                });
            }
            catch (NotAuthorizedException)
            {
                return Result<LoginResponse, string>.Fail("Invalid username or password.");
            }
            catch (UserNotConfirmedException)
            {
                return Result<LoginResponse, string>.Fail("User not confirmed.");
            }
            catch (Exception ex)
            {
                return Result<LoginResponse, string>.Fail($"Authentication error: {ex.Message}");
            }
        }

        public async Task<Result<LoginResponse, string>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var refreshRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                ClientId = clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "REFRESH_TOKEN", request.RefreshToken }
                }
            };

            try
            {
                var response = await client.InitiateAuthAsync(refreshRequest);
                return Result<LoginResponse, string>.Ok(new LoginResponse
                {
                    AccessToken = response.AuthenticationResult.AccessToken,
                    IdToken = response.AuthenticationResult.IdToken,
                    ExpiresIn = response.AuthenticationResult.ExpiresIn.GetValueOrDefault(),
                    RefreshToken = request.RefreshToken 
                });
            }
            catch (NotAuthorizedException)
            {
                return Result<LoginResponse, string>.Fail("Invalid refresh token.");
            }
            catch (Exception ex)
            {
                return Result<LoginResponse, string>.Fail($"Refresh token error: {ex.Message}");
            }
        }

        public async Task<Result<LoginResponse, string>> CompleteNewPasswordChallengeAsync(CompleteChallengeRequest request)
        {
            try
            {
                var challengeResponse = new RespondToAuthChallengeRequest
                {
                    ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                    ClientId = clientId,
                    Session = request.Session,
                    ChallengeResponses = new Dictionary<string, string>
                    {
                        { "USERNAME", request.Email },
                        { "NEW_PASSWORD", request.NewPassword }
                    }
                };

                var response = await client.RespondToAuthChallengeAsync(challengeResponse);
                var auth = response.AuthenticationResult;

                if (auth is null)
                    return Result<LoginResponse, string>.Fail("Challenge response succeeded, but no tokens were returned.");

                return Result<LoginResponse, string>.Ok(new LoginResponse
                {
                    AccessToken = auth.AccessToken,
                    RefreshToken = auth.RefreshToken,
                    IdToken = auth.IdToken,
                    ExpiresIn = auth.ExpiresIn.GetValueOrDefault()
                });
            }
            catch (Exception ex)
            {
                return Result<LoginResponse, string>.Fail($"Error completing challenge: {ex.Message}");
            }
        }
    }
}
