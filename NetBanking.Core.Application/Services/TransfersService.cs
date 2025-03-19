using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Application.Helpers;
using System.Transactions;

namespace NetBanking.Core.Application.Services
{
    public class TransfersService : ITransfersService
    {
        private readonly ITransactionService _transactionService;
        private readonly IClientService _clientService;
        private readonly ICreditCardService _creditCardService;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authResponse;
        private readonly IMapper _mapper;
        public TransfersService(ITransactionService transactionService, ISavingsAccountService savingsAccountService, IMapper mapper,
            IClientService clientService, IHttpContextAccessor httpContextAccessor, ICreditCardService creditCardService)
        {
            _transactionService = transactionService;
            _savingsAccountService = savingsAccountService;
            _mapper = mapper;
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _creditCardService = creditCardService;
        }
        public async Task<bool> AddMoneyToAccount(string accountNumberDestination, string accountNumberOrigin, double amount)
        {
            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);

            var destinyAccount = await _savingsAccountService.GetByAccountNumberLoggedUser(accountNumberDestination, client.Id);
            var originAccount = await _savingsAccountService.GetByAccountNumberLoggedUser(accountNumberOrigin, client.Id);

            if (destinyAccount != null && originAccount != null)
            {
                if (amount > destinyAccount.Balance)
                {
                    throw new Exception("You can't do this transaction, the amount is higher than the balance of the account.");
                }

                double destinyBalance = destinyAccount.Balance + amount;
                double originBalance = originAccount.Balance - amount;

                await _savingsAccountService.UpdateSavingsAccount(destinyBalance, client.Id, destinyAccount.Id);
                await _savingsAccountService.UpdateSavingsAccount(originBalance, client.Id, originAccount.Id);
                return true;
            }
            else
            {
                throw new Exception("Operation failed.");
            }
        }

        public async Task<bool> AddMoneyToAccountCreditCard(string accountNumberOrigin, string accountNumberDestination, double amount)
        {
            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);

            var destinyAccount = await _savingsAccountService.GetByAccountNumberLoggedUser(accountNumberDestination, client.Id);
            var originAccount = await _creditCardService.GetByAccountNumberLoggedUser(accountNumberOrigin, client.Id);

            if (destinyAccount != null && originAccount != null)
            {
                double destinyBalance = destinyAccount.Balance + amount;

                await _savingsAccountService.UpdateSavingsAccount(destinyBalance, client.Id, destinyAccount.Id);
                return true;
            }
            else
            {
                throw new Exception("Operation failed.");
            }
        }

        public async Task<bool> CashAdvance(SaveTransactionViewModel vm)
        {
            // Get credit card and verify if the limit is enough to make the cash advance
            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            var creditCard = await _creditCardService.GetByAccountNumberLoggedUser(vm.Origin, client.Id);
            if (creditCard == null)
            {
                throw new Exception("This credit card doesn't exist.");
            }

            if (vm.Amount > creditCard.Limit || vm.Amount > creditCard.Balance)
            {
                throw new Exception("You can't do this transaction, the amount is higher than the limit or the balance.");
            }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    double interest = 0.0625;
                    double debt = vm.Amount + (vm.Amount * interest);
                    double balance = creditCard.Balance - vm.Amount;

                    await _creditCardService.UpdateCreditCard(balance, debt, creditCard.Id, client.Id);
                    vm.TransactionTypeId = (int)Enums.TransactionType.CashAdvance;

                    if (vm.Concept == null || vm.Concept.Length == 0)
                    {
                        vm.Concept = "Cash advance";
                    }

                    var transferResult = await Transfer(vm, Enums.TransactionType.CashAdvance, true);

                    if (transferResult != null)
                    {
                        transactionScope.Complete();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Cash advance failed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception("An error occurred during cash advance.");
                }
            }
        }

        public async Task<SaveTransactionViewModel> Transfer(SaveTransactionViewModel vm, TransactionType transactionType, bool isCashAdvance)
        {
            vm.TransactionTypeId = (int)transactionType;
            vm.DateCreated = DateTime.Now;

            if (isCashAdvance)
            {
                var makeAdvance = await AddMoneyToAccountCreditCard(vm.Origin, vm.Destination, vm.Amount);
                if (!makeAdvance)
                {
                    return null;
                }

            }
            else
            {
                var addMoney = await AddMoneyToAccount(vm.Origin, vm.Destination, vm.Amount);
                if (!addMoney)
                {
                    return null;
                }

            }

            if (vm.Concept == null || vm.Concept.Length == 0)
            {
                vm.Concept = "Transfer";
            }

            return await _transactionService.Add(vm);
        }
    }
}
