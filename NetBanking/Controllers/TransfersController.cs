using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Application.Enums;

namespace WebApp.NetBanking.Controllers
{
    public class TransfersController : Controller
    {
        private readonly ITransfersService _transfersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly IClientService _clientService;
        private readonly AuthenticationResponse _authResponse;

        public TransfersController(ITransfersService transfersService, IHttpContextAccessor httpContextAccessor,
            ISavingsAccountService savingsAccountService, IClientService clientService, ICreditCardService creditCardService)
        {
            _transfersService = transfersService;
            _savingsAccountService = savingsAccountService;
            _clientService = clientService;
            _creditCardService = creditCardService;
            _httpContextAccessor = httpContextAccessor;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        public async Task<IActionResult> TransferBetweenAccounts(string? errorMessage = null)
        {

            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);

            var accounts = await _savingsAccountService.GetAllViewModel();
            accounts = accounts.Where(x => x.ClientId == client.Id).ToList();

            ViewBag.ClientAccounts = accounts;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransferBetweenAccounts(SaveTransactionViewModel vm)
        {
            try
            {
                var transfer = await _transfersService.Transfer(vm, TransactionType.AccountTransfer, false);
                if (transfer == null)
                {
                    return RedirectToRoute(new { controller = "Transfers", action = "TransferBetweenAccounts", errorMessage = "There was an error during the transfer." });
                }

            }
            catch (Exception ex)
            {

                return RedirectToRoute(new { controller = "Transfers", action = "TransferBetweenAccounts", errorMessage = ex.Message.ToString() });

            }

            return RedirectToRoute(new { controller = "Transfers", action = "TransferBetweenAccounts" });
        }

        public async Task<IActionResult> CashAdvance(string? errorMessage = null)
        {
            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);

            var creditCards = await _creditCardService.GetAllViewModel();
            creditCards = creditCards.Where(x => x.ClientId == client.Id).ToList();

            var accounts = await _savingsAccountService.GetAllViewModel();
            accounts = accounts.Where(x => x.ClientId == client.Id).ToList();

            ViewBag.ClientAccounts = accounts;
            ViewBag.ClientCreditCards = creditCards;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CashAdvance(SaveTransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CashAdvance");
            }
            try
            {
                var transfer = await _transfersService.CashAdvance(vm);
                if (!transfer)
                {
                    return RedirectToRoute(new { controller = "Transfers", action = "CashAdvance", errorMessage = "There was an error with the transaction" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Transfers", action = "CashAdvance", errorMessage = ex.Message.ToString() });
            }

            return RedirectToRoute(new { controller = "Transfers", action = "CashAdvance" });
        }
    }
}
