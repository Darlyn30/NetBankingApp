using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IAccountService _accountService;
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IHttpContextAccessor httpContextAccessor,
            IClientService clientService, IAccountService accountService, IMapper mapper) : base(beneficiaryRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _beneficiaryRepository = beneficiaryRepository;
            _clientService = clientService;
            _mapper = mapper;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region Create
        public async Task<SaveBeneficiaryViewModel> AddBeneficiary(SaveBeneficiaryViewModel vm, string AccountNumber)
        {
            var userId = user.Id;
            var client = await _clientService.GetByUserIdViewModel(userId);

            vm.ClientId = client.Id;
            vm.SavingsAccountId = AccountNumber;

            return await base.Add(vm);

        }
        #endregion

        #region Delete  
        public async Task DeleteBeneficiary(string accountNumber)
        {
            var beneficiary = await _beneficiaryRepository.GetBeneficiary(accountNumber);
            await base.Delete(beneficiary.Id);
        }
        #endregion

        #region Get Methods
        public async Task<List<BeneficiaryViewModel>> GetAllByClientId(int clientId)
        {
            var beneficiaries = await _beneficiaryRepository.GetAllAsync();

            var beneficiaryViewModels = new List<BeneficiaryViewModel>();

            foreach (var beneficiary in beneficiaries.Where(b => b.ClientId == clientId))
            {
                var account = await GetByAccountNumber(beneficiary.SavingsAccountId);
                var client = await _clientService.GetByIdSaveViewModel(account.ClientId);
                var clientUser = await _accountService.FindByIdAsync(client.UserId);

                var beneficiaryViewModel = new BeneficiaryViewModel
                {
                    ClientId = beneficiary.ClientId,
                    BeneficiaryAccountNumber = beneficiary.SavingsAccountId,
                    BeneficiaryName = clientUser.Name,
                    BeneficiaryLastName = clientUser.LastName
                };

                beneficiaryViewModels.Add(beneficiaryViewModel);
            }

            return beneficiaryViewModels;
        }


        public async Task<Beneficiary> GetBeneficiary(string accountNumber)
        {
            var beneficiary = await _beneficiaryRepository.GetBeneficiary(accountNumber);
            if (beneficiary == null)
            {
                return null;
            }

            return beneficiary;

        }

        public async Task<SavingsAccount> GetByAccountNumber(string accountNumber)
        {

            var sa = await _beneficiaryRepository.GetByAccountNumber(accountNumber);
            if (sa == null)

            {
                return null;
            }

            return sa;
        }

        #endregion
    }
}
