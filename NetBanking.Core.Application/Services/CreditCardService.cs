using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public CreditCardService(ICreditCardRepository creditCardRepository, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(creditCardRepository, mapper)
        {
            _creditCardRepository = creditCardRepository;
            _httpContextAccessor = httpContextAccessor;

            _mapper = mapper;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region Delete
        public async Task DeleteProduct(string id)
        {
            var crediCard = await GetByAccountNumber(id);

            await _creditCardRepository.DeleteAsync(crediCard);
        }
        #endregion

        #region Get Methods
        public async Task<CreditCard> GetByAccountNumber(string accountNumber)
        {
            var creditCard = await _creditCardRepository.GetByAccountNumber(accountNumber);
            if (creditCard == null)
            {
                return null;
            }

            return creditCard;
        }

        public async Task<List<CreditCardViewModel>> GetAllByClientId(int clientId)
        {
            var creditCards = await _creditCardRepository.GetAllAsync();

            var creditCardsViewModels = new List<CreditCardViewModel>();

            foreach (var creditCard in creditCards.Where(b => b.ClientId == clientId))
            {

                var creditCardViewModel = new CreditCardViewModel
                {
                    ClientId = creditCard.ClientId,
                    Id = creditCard.Id,
                    Balance = creditCard.Balance,
                    DateCreated = creditCard.DateCreated,
                    Debit = creditCard.Debit,
                    Limit = creditCard.Limit,
                };

                creditCardsViewModels.Add(creditCardViewModel);
            }

            return creditCardsViewModels;
        }

        public async Task<CreditCard> GetByAccountNumberLoggedUser(string accountNumber, int clientId)

        {
            var creditCard = await _creditCardRepository.GetByAccountNumberLoggedUser(accountNumber, clientId);
            if (creditCard == null)
            {
                return null;
            }

            return creditCard;
        }
        #endregion

        #region Update
        public async Task UpdateCreditCard(double balance, double debit, string accountNumber, int clientId)
        {
            var creditCard = await GetByAccountNumberLoggedUser(accountNumber, clientId);

            SaveCreditCardViewModel vm = new SaveCreditCardViewModel
            {
                Id = creditCard.Id,
                ClientId = creditCard.ClientId,
                DateCreated = creditCard.DateCreated,
                Balance = balance,
                Debit = debit,
                Limit = creditCard.Limit

            };

            await base.UpdateProduct(vm, accountNumber);
        }
        #endregion
    }
}
