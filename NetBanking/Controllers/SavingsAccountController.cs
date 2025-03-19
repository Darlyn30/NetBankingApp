using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Transaction;

namespace WebApp.NetBanking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SavingsAccountController : Controller
    {
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly ITransactionService _transactionService;

        public SavingsAccountController(ISavingsAccountService savingsAccountService, IProductService productService, IClientService clientService, ITransactionService transactionService)
        {
            _savingsAccountService = savingsAccountService;
            _productService = productService;
            _clientService = clientService;
            _transactionService = transactionService;
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(SaveSavingsAccountViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "SavingsAccount", action = "Create", hasError = true, message = "And error has ocurred trying to create a savings account: Not all the fields were correct." });
            }

            vm.Id = await _productService.GenerateProductNumber();
            await _savingsAccountService.Add(vm);

            var client = await _clientService.GetByIdSaveViewModel(vm.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var savingsAccount = await _savingsAccountService.GetByAccountNumber(id);

            //Transfer
            var mainAccount = await _savingsAccountService.GetClientMainAccount(savingsAccount.ClientId);
            SaveTransactionViewModel saveTransaction = new()
            {
                Origin = savingsAccount.Id,
                Destination = mainAccount.Id,
                TransactionTypeId = (int)TransactionType.AccountTransfer,
                Amount = savingsAccount.Balance,
                Concept = $"Delete the savings account: {savingsAccount.Id}",
                DateCreated = DateTime.UtcNow,
            };

            mainAccount.Balance += savingsAccount.Balance;
            await _transactionService.Add(saveTransaction);
            await _savingsAccountService.UpdateSavingsAccount(mainAccount.Balance, savingsAccount.ClientId, mainAccount.Id);

            await _savingsAccountService.Delete(id);

            var client = await _clientService.GetByIdSaveViewModel(savingsAccount.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });
        }
        #endregion
    }
}
