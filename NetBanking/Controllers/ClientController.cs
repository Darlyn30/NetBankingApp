using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;

namespace WebApp.NetBanking.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly AuthenticationResponse _authResponse;

        public ClientController(IHttpContextAccessor httpContextAccessor, IClientService clientService, IUserService userService)
        {

            _httpContextAccessor = httpContextAccessor;
            _authResponse = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _clientService = clientService;
            _userService = userService;
        }

        public async Task<IActionResult> Dashboard()
        {

            return View(await _clientService.GetAllProducts(_authResponse.Id));

        }
    }
}
