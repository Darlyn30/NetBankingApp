using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.CreditCard;

namespace WebApp.NetBanking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CreditCardController : Controller
    {
        private readonly ICreditCardService _creditCardService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;

        public CreditCardController(ICreditCardService creditCardService, IProductService productService, IClientService clientService)
        {
            _creditCardService = creditCardService;
            _productService = productService;
            _clientService = clientService;
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(SaveCreditCardViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "CreditCard", action = "Create", hasError = true, message = "And error has ocurred trying to create a credit card: Not all the fields were correct." });
            }

            vm.Id = await _productService.GenerateProductNumber();
            vm.Balance = vm.Limit;
            await _creditCardService.Add(vm);

            var client = await _clientService.GetByIdSaveViewModel(vm.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var creditCard = await _creditCardService.GetByAccountNumber(id);

            await _creditCardService.DeleteProduct(id);

            var client = await _clientService.GetByIdSaveViewModel(creditCard.ClientId);

            return RedirectToRoute(new { controller = "Admin", action = "IndexProducts", userId = client.UserId });

        }
        #endregion
    }
}
