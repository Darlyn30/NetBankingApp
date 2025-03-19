using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.ViewModels.User;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm);
        Task<Responses> UpdateUserAsync(SaveUserViewModel vm);
        Task<SaveUserViewModel> GetUpdateUserAsync(string username);
        Task SignOutAsync();
        Task<UserViewModel> GetByUsername(string username);
        Task<UserViewModel> GetById(string id);
    }
}
