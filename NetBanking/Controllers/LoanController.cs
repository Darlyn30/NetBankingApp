using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.Transaction;

namespace WebApp.NetBanking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ITransactionService _transactionService;

        public LoanController(ILoanService loanService, IProductService productService, IClientService clientService, ITransactionService transactionService, ISavingsAccountService savingsAccountService)
        {
            _loanService = loanService;
            _productService = productService;
            _clientService = clientService;
            _transactionService = transactionService;
            _savingsAccountService = savingsAccountService;
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(SaveLoanViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "Loan", action = "Create", hasError = true, message = "And error has ocurred trying to create a loan: Not all the fields were correct." });
            }

            vm.Id = await _productService.GenerateProductNumber();
            vm.Balance = vm.Amount;
            var loan = await _loanService.Add(vm);

            //Transfer
            var mainAccount = await _savingsAccountService.GetClientMainAccount(loan.ClientId);
            SaveTransactionViewModel saveTransaction = new()
            {
                Origin = loan.Id,
                Destination = mainAccount.Id,
                TransactionTypeId = (int)TransactionType.AccountTransfer,
                Amount = loan.Balance.Value,
                Concept = $"New loan: {loan.Id}"
            };

            mainAccount.Balance += loan.Balance.Value;
            await _transactionService.Add(saveTransaction);
            await _savingsAccountService.UpdateSavingsAccount(mainAccount.Balance, vm.ClientId, mainAccount.Id);

            var client = await _clientService.GetByIdSaveViewModel(vm.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var loan = await _loanService.GetByAccountNumber(id);

            await _loanService.DeleteProduct(id);

            var client = await _clientService.GetByIdSaveViewModel(loan.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });
        }
        #endregion
    }
}
