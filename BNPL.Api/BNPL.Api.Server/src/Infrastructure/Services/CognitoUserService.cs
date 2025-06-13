using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Application.DTOs.User;
using Core.Models;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class CognitoUserService(
        IAmazonCognitoIdentityProvider client,
        IConfiguration config
    ) : ICognitoUserService
    {
        private readonly string userPoolId = config["AWS:Cognito:UserPoolId"]
            ?? throw new InvalidOperationException("Cognito UserPoolId not configured.");

        public async Task<Result<bool, Error>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                await client.AdminCreateUserAsync(new AdminCreateUserRequest
                {
                    UserPoolId = userPoolId,
                    Username = request.Email,
                    TemporaryPassword = request.Password,
                    MessageAction = MessageActionType.SUPPRESS,
                    UserAttributes =
                    [
                        new() { Name = "email", Value = request.Email },
                        new() { Name = "email_verified", Value = "true" },
                        .. request.Attributes?.Select(x => new AttributeType { Name = x.Key, Value = x.Value }) ?? []
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

                return Result<bool, Error>.Ok(true);
            }
            catch (UsernameExistsException) { return Result<bool, Error>.Fail(DomainErrors.User.AlreadyExists); }
            catch (ResourceNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.Group.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.CreationError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> DeleteUserAsync(string email)
        {
            try
            {
                await client.AdminDeleteUserAsync(new() { Username = email, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.DeletionError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> EnableUserAsync(string email)
        {
            try
            {
                await client.AdminEnableUserAsync(new() { Username = email, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.EnableError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> DisableUserAsync(string email)
        {
            try
            {
                await client.AdminDisableUserAsync(new() { Username = email, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.DisableError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> ResetPasswordAsync(string email)
        {
            try
            {
                await client.AdminResetUserPasswordAsync(new() { Username = email, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.ResetPasswordError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> AddToGroupAsync(string email, string group)
        {
            try
            {
                await client.AdminAddUserToGroupAsync(new() { Username = email, GroupName = group, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (ResourceNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.Group.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.AddToGroupError", ex.Message)); }
        }

        public async Task<Result<bool, Error>> RemoveFromGroupAsync(string email, string group)
        {
            try
            {
                await client.AdminRemoveUserFromGroupAsync(new() { Username = email, GroupName = group, UserPoolId = userPoolId });
                return Result<bool, Error>.Ok(true);
            }
            catch (UserNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.User.NotFound); }
            catch (ResourceNotFoundException) { return Result<bool, Error>.Fail(DomainErrors.Group.NotFound); }
            catch (Exception ex) { return Result<bool, Error>.Fail(new Error("User.RemoveFromGroupError", ex.Message)); }
        }

        public async Task<Result<IReadOnlyCollection<string>, Error>> GetUserGroupsAsync(string email)
        {
            try
            {
                var result = await client.AdminListGroupsForUserAsync(new() { Username = email, UserPoolId = userPoolId });
                return Result<IReadOnlyCollection<string>, Error>.Ok([.. result.Groups.Select(g => g.GroupName)]);
            }
            catch (UserNotFoundException) { return Result<IReadOnlyCollection<string>, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<IReadOnlyCollection<string>, Error>.Fail(new Error("User.GetGroupsError", ex.Message)); }
        }

        public async Task<Result<IReadOnlyCollection<string>, Error>> ListGroupsAsync()
        {
            try
            {
                var result = await client.ListGroupsAsync(new() { UserPoolId = userPoolId });
                return Result<IReadOnlyCollection<string>, Error>.Ok([.. result.Groups.Select(g => g.GroupName)]);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<string>, Error>.Fail(new Error("Group.ListError", ex.Message));
            }
        }

        public async Task<Result<UserResponse, Error>> GetUserAsync(string email)
        {
            try
            {
                var result = await client.AdminGetUserAsync(new() { Username = email, UserPoolId = userPoolId });
                var groupsResult = await GetUserGroupsAsync(email);

                if (groupsResult.TryGetError(out var error))
                {
                    return Result<UserResponse, Error>.Fail(error);
                }

                groupsResult.TryGetSuccess(out var groups);

                return Result<UserResponse, Error>.Ok(new UserResponse
                {
                    Email = email,
                    Enabled = result.Enabled.GetValueOrDefault(),
                    Confirmed = result.UserStatus == UserStatusType.CONFIRMED,
                    Attributes = result.UserAttributes.ToDictionary(a => a.Name, a => a.Value),
                    Groups = groups ?? []
                });
            }
            catch (UserNotFoundException) { return Result<UserResponse, Error>.Fail(DomainErrors.User.NotFound); }
            catch (Exception ex) { return Result<UserResponse, Error>.Fail(new Error("User.GetError", ex.Message)); }
        }

        public async Task<Result<IReadOnlyCollection<UserResponse>, Error>> ListUsersAsync()
        {
            try
            {
                var result = await client.ListUsersAsync(new() { UserPoolId = userPoolId });
                var userTasks = result.Users.Select(async user =>
                {
                    var email = user.Attributes.FirstOrDefault(a => a.Name == "email")?.Value ?? user.Username;
                    var groupsResult = await GetUserGroupsAsync(user.Username);
                    groupsResult.TryGetSuccess(out var groups);

                    return new UserResponse
                    {
                        Email = email,
                        Enabled = user.Enabled.GetValueOrDefault(),
                        Confirmed = user.UserStatus == UserStatusType.CONFIRMED,
                        Attributes = user.Attributes.ToDictionary(a => a.Name, a => a.Value),
                        Groups = groups ?? []
                    };
                });

                var users = await Task.WhenAll(userTasks);
                return Result<IReadOnlyCollection<UserResponse>, Error>.Ok(users);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyCollection<UserResponse>, Error>.Fail(new Error("User.ListError", ex.Message));
            }
        }
    }
}