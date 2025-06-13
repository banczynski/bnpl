using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.User;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class CognitoUserService(
        IAmazonCognitoIdentityProvider client,
        IConfiguration config
    ) : ICognitoUserService
    {
        private readonly string userPoolId = config["AWS:Cognito:UserPoolId"]
            ?? throw new InvalidOperationException("Cognito UserPoolId not configured.");

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            await client.AdminCreateUserAsync(new AdminCreateUserRequest
            {
                UserPoolId = userPoolId,
                Username = request.Email,
                TemporaryPassword = request.Password,
                MessageAction = MessageActionType.SUPPRESS,
                UserAttributes = [
                    new() { Name = "email", Value = request.Email },
                    new() { Name = "email_verified", Value = "true" },
                    ..request.Attributes?.Select(x => new AttributeType { Name = x.Key, Value = x.Value }) ?? []
                ]
            });

            await client.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest
            {
                Username = request.Email,
                Password = request.Password,
                Permanent = true,
                UserPoolId = userPoolId
            });

            await client.AdminConfirmSignUpAsync(new AdminConfirmSignUpRequest
            {
                Username = request.Email,
                UserPoolId = userPoolId
            });

            await client.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                Username = request.Email,
                GroupName = request.Group,
                UserPoolId = userPoolId
            });
        }

        public Task DeleteUserAsync(string email) =>
            client.AdminDeleteUserAsync(new() { Username = email, UserPoolId = userPoolId });

        public Task EnableUserAsync(string email) =>
            client.AdminEnableUserAsync(new() { Username = email, UserPoolId = userPoolId });

        public Task DisableUserAsync(string email) =>
            client.AdminDisableUserAsync(new() { Username = email, UserPoolId = userPoolId });

        public Task ResetPasswordAsync(string email) =>
            client.AdminResetUserPasswordAsync(new() { Username = email, UserPoolId = userPoolId });

        public Task AddToGroupAsync(string email, string group) =>
            client.AdminAddUserToGroupAsync(new() { Username = email, GroupName = group, UserPoolId = userPoolId });

        public Task RemoveFromGroupAsync(string email, string group) =>
            client.AdminRemoveUserFromGroupAsync(new() { Username = email, GroupName = group, UserPoolId = userPoolId });

        public async Task<IReadOnlyCollection<string>> GetUserGroupsAsync(string email)
        {
            var result = await client.AdminListGroupsForUserAsync(new() { Username = email, UserPoolId = userPoolId });
            return [.. result.Groups.Select(g => g.GroupName)];
        }

        public async Task<IReadOnlyCollection<string>> ListGroupsAsync()
        {
            var result = await client.ListGroupsAsync(new() { UserPoolId = userPoolId });
            return [.. result.Groups.Select(g => g.GroupName)];
        }

        public async Task<UserResponse> GetUserAsync(string email)
        {
            var result = await client.AdminGetUserAsync(new() { Username = email, UserPoolId = userPoolId });
            var groups = await GetUserGroupsAsync(email);

            return new UserResponse
            {
                Email = email,
                Enabled = result.Enabled.GetValueOrDefault(),
                Confirmed = result.UserStatus == UserStatusType.CONFIRMED,
                Attributes = result.UserAttributes.ToDictionary(a => a.Name, a => a.Value),
                Groups = groups
            };
        }

        public async Task<IReadOnlyCollection<UserResponse>> ListUsersAsync()
        {
            var result = await client.ListUsersAsync(new() { UserPoolId = userPoolId });

            var users = new List<UserResponse>();
            foreach (var user in result.Users)
            {
                var email = user.Attributes.FirstOrDefault(a => a.Name == "email")?.Value ?? user.Username;
                var groups = await GetUserGroupsAsync(user.Username);

                users.Add(new UserResponse
                {
                    Email = email,
                    Enabled = user.Enabled.GetValueOrDefault(),
                    Confirmed = user.UserStatus == UserStatusType.CONFIRMED,
                    Attributes = user.Attributes.ToDictionary(a => a.Name, a => a.Value),
                    Groups = groups
                });
            }

            return users;
        }
    }
}
