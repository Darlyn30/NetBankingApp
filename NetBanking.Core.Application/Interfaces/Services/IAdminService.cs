using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.ViewModels.User;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetAllViewModel();
        Task<Responses> UpdateUserStatus(string userId);
        Task<int> GetActiveUsersCount();
        Task<int> GetInactiveUsersCount();
        Task<int> GetTotalTransactionsCount();
        Task<int> GetTodayTotalTransactionsCount();
        Task<int> GetTotalLoansCount();
        Task<int> GetTotalSavingsAccountsCount();
        Task<int> GetTotalCreditCardsCount();
    }
}
