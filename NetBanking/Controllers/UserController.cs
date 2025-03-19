using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Infrastructure.Identity.Entities;
using NetBanking.Core.Application.Helpers;
using NetBanking.Middlewares;
using NetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;

namespace NetBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly AuthenticationResponse authVm;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            authVm = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region LogIn
        [ServiceFilter(typeof(LoginAuthorize))]

        public async Task<IActionResult> Index(bool hasError = false, string? message = null)
        {
            var login = new LoginViewModel();

            if (hasError)
            {
                login.HasError = hasError;
                login.Error = message;
            }

            return View(login);
        }

        public async Task<IActionResult> RedirectIndex(string? returnUrl)
        {
            return RedirectToRoute(new { controller = "User", action = "Index", hasError = true, message = "You don't have access to this section!" });

        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set("user", userVm);
                if (userVm.Roles.Contains("Admin"))
                {

                    return RedirectToRoute(new { controller = "Admin", action = "Index" });
                }
                else
                {
                    return RedirectToRoute(new { controller = "Client", action = "Dashboard" });
                }
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        #endregion

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
