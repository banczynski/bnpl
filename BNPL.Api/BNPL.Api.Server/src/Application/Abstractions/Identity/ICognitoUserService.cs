using BNPL.Api.Server.src.Application.DTOs.User;
using Core.Models;

namespace BNPL.Api.Server.src.Application.Abstractions.Identity
{
    public interface ICognitoUserService
    {
        Task<Result<bool, Error>> CreateUserAsync(CreateUserRequest request);
        Task<Result<bool, Error>> DeleteUserAsync(string email);
        Task<Result<bool, Error>> EnableUserAsync(string email);
        Task<Result<bool, Error>> DisableUserAsync(string email);
        Task<Result<bool, Error>> ResetPasswordAsync(string email);
        Task<Result<bool, Error>> AddToGroupAsync(string email, string group);
        Task<Result<bool, Error>> RemoveFromGroupAsync(string email, string group);
        Task<Result<IReadOnlyCollection<string>, Error>> GetUserGroupsAsync(string email);
        Task<Result<IReadOnlyCollection<string>, Error>> ListGroupsAsync();
        Task<Result<UserResponse, Error>> GetUserAsync(string email);
        Task<Result<IReadOnlyCollection<UserResponse>, Error>> ListUsersAsync();
    }
}