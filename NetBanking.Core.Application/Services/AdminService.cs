using AutoMapper;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.User;

namespace NetBanking.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ILoanService _loanService;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly IMapper _mapper;

        public AdminService(IAccountService accountService, ITransactionService transactionService, ILoanService loanService, ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, IMapper mapper)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _loanService = loanService;
            _savingsAccountService = savingsAccountService;
            _creditCardService = creditCardService;
            _mapper = mapper;
        }

        #region Dashboard Methods
        public async Task<int> GetActiveUsersCount()
        {
            return await _accountService.GetActiveUsersCountAsync();
        }

        public async Task<int> GetInactiveUsersCount()
        {
            return await _accountService.GetInactiveUsersCountAsync();
        }

        public async Task<int> GetTotalTransactionsCount()
        {
            var transactions = await _transactionService.GetAllViewModel();
            if (transactions == null)
            {
                return 0;
            }

            return transactions.Count;
        }

        public async Task<int> GetTodayTotalTransactionsCount()
        {
            var transactions = await _transactionService.GetAllViewModel();
            transactions = transactions.Where(x => x.DateCreated.Day == DateTime.Now.Day).ToList();

            if (transactions == null)
            {
                return 0;
            }

            return transactions.Count;
        }

        public async Task<int> GetTotalLoansCount()
        {
            var loans = await _loanService.GetAllViewModel();
            if (loans == null)
            {
                return 0;
            }

            return loans.Count;
        }

        public async Task<int> GetTotalSavingsAccountsCount()
        {
            var savingsAccounts = await _savingsAccountService.GetAllViewModel();
            if (savingsAccounts == null)
            {
                return 0;
            }


            return savingsAccounts.Count;
        }

        public async Task<int> GetTotalCreditCardsCount()
        {
            var creditCards = await _creditCardService.GetAllViewModel();
            if (creditCards == null)
            {
                return 0;
            }
            return creditCards.Count;
        }
        #endregion

        #region GetAllViewModel
        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            var userList = await _accountService.GetAllUserAsync();

            return _mapper.Map<List<UserViewModel>>(userList);
        }
        #endregion

        #region UpdateUserStatus
        public async Task<Responses> UpdateUserStatus(string userId)
        {
            var userStatus = await _accountService.UpdateUserStatusAsync(userId);

            return userStatus;
        }
        #endregion
    }
}
