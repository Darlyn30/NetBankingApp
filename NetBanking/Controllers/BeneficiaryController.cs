using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.Beneficiary;

namespace WebApp.NetBanking.Controllers
{
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authResponse;

        public BeneficiaryController(IBeneficiaryService beneficiaryService, IHttpContextAccessor httpContextAccessor, IClientService clientService)
        {
            _beneficiaryService = beneficiaryService;
            _httpContextAccessor = httpContextAccessor;
            _clientService = clientService;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index()
        {

            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            var vms = await _beneficiaryService.GetAllByClientId(client.Id);

            return View(vms);
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiaryBySearch(string accountNumber)
        {

            // Here goes the method to get the user's account number
            var client = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            var acc = await _beneficiaryService.GetByAccountNumber(accountNumber);

            if (acc == null)
            {
                ModelState.AddModelError("AccountNumber", "Account not found.");
                return RedirectToAction("Index");
            }

            if (acc.ClientId == client.Id && acc.Id == accountNumber)
            {
                ModelState.AddModelError("AccountNumber", "You cannot add yourself as a beneficiary.");
                return RedirectToAction("Index");
            }

            var beneficiaries = await _beneficiaryService.GetAllByClientId(client.Id);
            if (beneficiaries.Any(f => (f.ClientId == client.Id && f.BeneficiaryAccountNumber == accountNumber)))
            {
                ModelState.AddModelError("AccountNumber", "This person is already a beneficiary.");
                return RedirectToAction("Index");
            }

            SaveBeneficiaryViewModel vm = new();
            await _beneficiaryService.AddBeneficiary(vm, accountNumber);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteBeneficiary(string accountNumber)
        {

            return View(await _beneficiaryService.GetBeneficiary(accountNumber));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBeneficiaryPost(string SavingsAccountId)
        {
            await _beneficiaryService.DeleteBeneficiary(SavingsAccountId);
            return RedirectToAction("Index");
        }
    }
}
