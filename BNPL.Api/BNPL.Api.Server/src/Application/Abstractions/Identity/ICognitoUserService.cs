using BNPL.Api.Server.src.Application.DTOs.User;

namespace BNPL.Api.Server.src.Application.Abstractions.Identity
{
    public interface ICognitoUserService
    {
        Task CreateUserAsync(CreateUserRequest request);
        Task DeleteUserAsync(string email);
        Task EnableUserAsync(string email);
        Task DisableUserAsync(string email);
        Task ResetPasswordAsync(string email);
        Task AddToGroupAsync(string email, string group);
        Task RemoveFromGroupAsync(string email, string group);
        Task<IReadOnlyCollection<string>> GetUserGroupsAsync(string email);
        Task<IReadOnlyCollection<string>> ListGroupsAsync();
        Task<UserResponse> GetUserAsync(string email);
        Task<IReadOnlyCollection<UserResponse>> ListUsersAsync();
    }
}
