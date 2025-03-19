using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.ViewModels.Client;
using NetBanking.Core.Application.ViewModels.Product;
using NetBanking.Core.Application.ViewModels.User;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IClientService : IGenericService<SaveClientViewModel, ClientViewModel, Client>
    {
        Task<ClientViewModel> GetByUserIdViewModel(string userId);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm);
        Task<Responses> UpdateAsync(SaveUserViewModel vm);
        Task<ProductViewModel> GetAllProducts(string userId);
        Task<Client> GetByAccountNumber(string accountNumber);
    }
}
