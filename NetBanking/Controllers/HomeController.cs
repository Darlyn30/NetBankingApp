using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.User;

namespace NetBanking.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthenticationResponse _authResponse;

    public HomeController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
    }

    public IActionResult Index()
    {
        var isAdmin  = _authResponse != null ? _authResponse.Roles.Any(r => r == "Admin") : false;
        var isClient = _authResponse != null ? _authResponse.Roles.Any(r => r == "Client") : false;

        if (isAdmin)
        {
            return RedirectToAction("Index", "Admin");
        }
        else if(isClient)
        {
            return RedirectToAction("Dashboard", "Client");
        }
        else
        {
            return RedirectToAction("RedirectIndex", "User");
        }
    }
}
