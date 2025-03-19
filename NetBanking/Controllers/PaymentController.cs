using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.Payment;

namespace WebApp.NetBanking.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authResponse;
        private readonly IPaymentService _paymentService;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IAccountService _accountService;
        private readonly IClientService _clientService;

        public PaymentController(IHttpContextAccessor httpContextAccessor, IPaymentService paymentService,
            ISavingsAccountService savingsAccountService, ICreditCardService creditCardService, ILoanService loanService,
            IBeneficiaryService beneficiaryService, IClientService clientService, IAccountService accountService)
        {
            _savingsAccountService = savingsAccountService;
            _accountService = accountService;
            _clientService = clientService;
            _creditCardService = creditCardService;
            _loanService = loanService;
            _beneficiaryService = beneficiaryService;
            _httpContextAccessor = httpContextAccessor;
            _paymentService = paymentService;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> ExpressPayment(string? errorMessage = null)
        {

            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            ExpressPaymentViewModel vm = new();

            vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
            return View("ExpressPayment", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(ExpressPaymentViewModel vm)
        {
            try
            {
                if (vm.Destination.Length < 9 || vm.Destination.Length > 9 || vm.Destination == null)
                {
                    throw new Exception("Destination account does not exist");

                }
                if (!ModelState.IsValid)
                {



                    return RedirectToRoute(new { controller = "Client", action = "Dashboard" });

                }
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
                var destinyClient = await _clientService.GetByAccountNumber(vm.Destination);
                var destinyUser = await _accountService.FindByIdAsync(destinyClient.UserId);

                vm.ClientName = destinyUser.Name;
                vm.ClientLastName = destinyUser.LastName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToRoute(new { controller = "Payment", action = "ExpressPayment", errorMessage = ex.Message.ToString() });
            }

            return View("ExpressPaymentConfirm", vm);
        }

        public async Task<IActionResult> ExpressPaymentConfirm(ExpressPaymentViewModel vm, string? errorMessage = null)
        {

            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            if (!ModelState.IsValid)
            {


                return RedirectToRoute(new { controller = "Client", action = "Dashboard" });


            }

            return View("ExpressPaymentConfirm", vm);
        }

        [HttpPost]

        public async Task<IActionResult> ExpressPaymentConfirmPost(ExpressPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
                vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
                return View("ExpressPaymentConfirm", vm);
            }
            try
            {
                await _paymentService.ExpressPayment(vm);
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Client", action = "Dashboard", errorMessage = ex.Message.ToString() });

            }


            return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
        }

        public async Task<IActionResult> BeneficiaryPayment(string? errorMessage = null)
        {
            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            BeneficiaryPaymentViewModel vm = new();
            vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
            vm.LoggedUserBeneficiaries = await _beneficiaryService.GetAllByClientId(loggedClient.Id);
            return View("BeneficiaryPayment", vm);

        }

        [HttpPost]
        public async Task<IActionResult> BeneficiaryPayment(BeneficiaryPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {

                return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
            }
            try
            {
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
                var destinyClient = await _clientService.GetByAccountNumber(vm.Destination);
                var destinyUser = await _accountService.FindByIdAsync(destinyClient.UserId);

                vm.BeneficiaryFirstName = destinyUser.Name;
                vm.BeneficiaryLastName = destinyUser.LastName;
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
            }


            return View("BeneficiaryPaymentConfirm", vm);
        }

        public async Task<IActionResult> BeneficiaryPaymentConfirm(BeneficiaryPaymentViewModel vm, string? errorMessage = null)
        {
            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }


            return View("BeneficiaryPaymentConfirm", vm);
        }

        [HttpPost]

        public async Task<IActionResult> BeneficiaryPaymentConfirmPost(BeneficiaryPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
                vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
                vm.LoggedUserBeneficiaries = await _beneficiaryService.GetAllByClientId(loggedClient.Id);
                return View("BeneficiaryPaymentConfirm", vm);
            }

            try
            {
                await _paymentService.BeneficiaryPayment(vm);
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Client", action = "Dashboard", errorMessage = ex.Message.ToString() });
            }

            return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
        }

        public async Task<IActionResult> LoanPayment(string? errorMessage = null)
        {
            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            LoanPaymentViewModel vm = new();
            vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
            vm.LoggedUserLoans = await _loanService.GetAllByClientId(loggedClient.Id);
            return View("LoanPayment", vm);
        }

        [HttpPost]
        public async Task<IActionResult> LoanPayment(LoanPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
                vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
                vm.LoggedUserLoans = await _loanService.GetAllByClientId(loggedClient.Id);
                return View("LoanPayment", vm);
            }
            try
            {
                await _paymentService.LoanPayment(vm);
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Client", action = "Dashboard", errorMessage = ex.Message.ToString() });
            }




            return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
        }

        public async Task<IActionResult> CreditCardPayment(string? errorMessage = null)
        {
            ViewBag.Error = "";
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }

            var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);
            CreditCardPaymentViewModel vm = new();
            vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
            vm.LoggedUserCreditCards = await _creditCardService.GetAllByClientId(loggedClient.Id);
            return View("CreditCardPayment", vm);



        }

        [HttpPost]
        public async Task<IActionResult> CreditCardPayment(CreditCardPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var loggedClient = await _clientService.GetByUserIdViewModel(_authResponse.Id);

                vm.LoggedUserAccounts = await _savingsAccountService.GetAllByClientId(loggedClient.Id);
                vm.LoggedUserCreditCards = await _creditCardService.GetAllByClientId(loggedClient.Id);
                return View("CreditCardPayment", vm);
            }
            try
            {


                await _paymentService.CreditCardPayment(vm);


            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Client", action = "Dashboard", errorMessage = ex.Message.ToString() });

            }
            return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
        }
    }
}
