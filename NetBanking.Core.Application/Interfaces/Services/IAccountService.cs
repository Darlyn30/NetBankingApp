using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
        Task<Responses> UpdateUserAsync(UpdateUserRequest request);
        Task SignOutAsync();
        Task<List<UserDTO>> GetAllUserAsync();
        Task<UserDTO> FindByUsernameAsync(string username);
        Task<UserDTO> FindByIdAsync(string id);
        Task<Responses> UpdateUserStatusAsync(string userId);
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetInactiveUsersCountAsync();
    }
}
