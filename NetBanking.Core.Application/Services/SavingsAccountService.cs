using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authResponse;

        public SavingsAccountService(ISavingsAccountRepository savingsAccountRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper,
            IAccountService accountService) : base(savingsAccountRepository, mapper)
        {
            _savingsAccountRepository = savingsAccountRepository;
            _mapper = mapper;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task Delete(string id)
        {
            var savingsAccount = await GetByAccountNumber(id);

            await _savingsAccountRepository.DeleteAsync(savingsAccount);
        }

        public async Task<List<SavingsAccountViewModel>> GetAllByClientId(int clientId)
        {
            var sas = await _savingsAccountRepository.GetAllAsync();

            var savingsAccountsVm  = new List<SavingsAccountViewModel>();

            foreach (var sa in sas.Where(b => b.ClientId == clientId))
            {

                var saViewModel = new SavingsAccountViewModel
                {
                    ClientId = sa.ClientId,
                    Id = sa.Id,
                    Balance = sa.Balance,
                    DateCreated = sa.DateCreated,
                    IsMainAccount = sa.IsMainAccount



                };

                savingsAccountsVm.Add(saViewModel);
            }

            return savingsAccountsVm;
        }

        public async Task<SavingsAccount> GetByAccountNumber(string accountNumber)
        {
            var savingsAccount = await _savingsAccountRepository.GetByAccountNumber(accountNumber);
            if (savingsAccount == null)
            {
                return null;
            }

            return savingsAccount;
        }

        public async Task<SavingsAccount> GetByAccountNumberLoggedUser(string accountNumber, int ClientId)
        {
            var savingsAccount = await _savingsAccountRepository.GetByAccountNumberLoggedUser(accountNumber, ClientId);
            if (savingsAccount == null)
            {
                return null;
            }

            return savingsAccount;
        }

        public async Task<SavingsAccountViewModel> GetClientMainAccount(int ClientId)
        {
            var savingsAccountList = await base.GetAllViewModel();

            var savingsAccount = savingsAccountList.FirstOrDefault(sa => sa.ClientId == ClientId && sa.IsMainAccount == true);

            return savingsAccount;
        }

        public async Task UpdateSavingsAccount(double balance, int ClientId, string id)
        {
            var savingsAccount = await GetByAccountNumberLoggedUser(id, ClientId);

            SaveSavingsAccountViewModel vm = new SaveSavingsAccountViewModel
            {
                Id = savingsAccount.Id,
                ClientId = ClientId,
                DateCreated = savingsAccount.DateCreated,
                Balance = balance,
                IsMainAccount = savingsAccount.IsMainAccount
            };

            await base.UpdateProduct(vm, id);
        }
    }
}
