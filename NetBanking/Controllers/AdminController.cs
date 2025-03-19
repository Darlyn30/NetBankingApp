using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using WebApp.NetBanking.Models;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.ViewModels.User;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;

namespace WebApp.NetBanking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IAdminService _adminService;
        private readonly AuthenticationResponse _authResponse;

        public AdminController(IHttpContextAccessor httpContextAccessor, IUserService userService, IClientService clientService, IAdminService adminService, ITransactionService transactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userService = userService;
            _clientService = clientService;
            _adminService = adminService;
            _transactionService = transactionService;
        }

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {

            var payments = await _transactionService.GetAllViewModel();
            var loan = payments.Where(t => t.TransactionTypeId == (int)TransactionType.LoanPayment).ToList();
            var express = payments.Where(t => t.TransactionTypeId == (int)TransactionType.ExpressPayment).ToList();
            var creditcard = payments.Where(t => t.TransactionTypeId == (int)TransactionType.CreditCardPayment).ToList();
            var beneficiary = payments.Where(t => t.TransactionTypeId == (int)TransactionType.BeneficiaryPayment).ToList();

            var totalPayments = loan.Count + express.Count + creditcard.Count + beneficiary.Count;
            var totalTodayPayments = payments
                                    .Where(x => x.DateCreated.Day == DateTime.Now.Day &&
                                                (x.TransactionTypeId == (int)TransactionType.LoanPayment ||
                                                 x.TransactionTypeId == (int)TransactionType.ExpressPayment ||
                                                 x.TransactionTypeId == (int)TransactionType.CreditCardPayment ||
                                                 x.TransactionTypeId == (int)TransactionType.BeneficiaryPayment))
                                    .ToList()
                                    .Count;


            AdminDataViewModel vm = new()
            {
                TotalUsers = await _adminService.GetActiveUsersCount() + await _adminService.GetInactiveUsersCount(),
                TotalActiveUsers = await _adminService.GetActiveUsersCount(),
                TotalInactiveUsers = await _adminService.GetInactiveUsersCount(),
                TotalTransactions = await _adminService.GetTotalTransactionsCount(),
                TotalTodayTransactions = await _adminService.GetTodayTotalTransactionsCount(),
                TotalPayments = totalPayments,
                TotalTodayPayments = totalTodayPayments,
                TotalSavingsAccounts = await _adminService.GetTotalSavingsAccountsCount(),
                TotalLoans = await _adminService.GetTotalLoansCount(),
                TotalCreditCards = await _adminService.GetTotalCreditCardsCount()
            };


            return View(vm);
        }
        #endregion

        #region GetAllUsers
        public async Task<IActionResult> Index(bool hasError = false, string? message = null)
        {
            List<UserViewModel> users = await _adminService.GetAllViewModel();
            ViewBag.User = _authResponse;
            Responses response = new()
            {
                HasError = hasError,
            };

            if (hasError)
                response.Error = message;
            ViewBag.Response = response;

            return View(users);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            return View("SaveUser", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUser", vm);
            }

            RegisterResponse response = new();

            if (vm.Role == (int)Roles.Admin)
            {
                response = await _userService.RegisterAsync(vm);
            }
            else if (vm.Role == (int)Roles.Client)
            {
                response = await _clientService.RegisterAsync(vm);
            }

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveUser", vm);
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string username)
        {
            var response = await _adminService.UpdateUserStatus(username);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = "Admin", action = "Index", hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index", });
        }
        #endregion

        #region Edit User
        public async Task<IActionResult> Edit(string username)
        {
            SaveUserViewModel vm = await _userService.GetUpdateUserAsync(username);

            return View("SaveUser", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUser", vm);
            }

            Responses response = new();

            if (vm.Role == (int)Roles.Admin)
            {
                response = await _userService.UpdateUserAsync(vm);
            }
            else if (vm.Role == (int)Roles.Client)
            {
                response = await _clientService.UpdateAsync(vm);
            }

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveUser", vm);
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Products
        public async Task<IActionResult> IndexProducts(string userId, bool hasError = false, string? message = null)
        {
            try
            {
                ViewBag.user = _authResponse;
                Responses response = new()
                {
                    HasError = hasError,
                };

                if (hasError)
                    response.Error = message;
                ViewBag.Response = response;

                return View(await _clientService.GetAllProducts(userId));
            }
            catch (Exception ex)
            {
                var user = await _userService.GetById(userId);

                return RedirectToRoute(new { controller = "Admin", action = "Index", userId = user.Id, hasError = true, message = $"An error has occured trying to get the products of the client: {user.Username}. Try to update the client main account on edit." });
            }
        }
        #endregion


    }
}
