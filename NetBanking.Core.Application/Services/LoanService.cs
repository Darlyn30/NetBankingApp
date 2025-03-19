using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services
{
    public class LoanService : GenericService<SaveLoanViewModel, LoanViewModel, Loan>, ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper) : base(loanRepository, mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        #region Delete
        public async Task DeleteProduct(string id)
        {
            var loan = await GetByAccountNumber(id);

            await _loanRepository.DeleteAsync(loan);
        }
        #endregion

        #region Get Methods
        public async Task<Loan> GetByAccountNumber(string accountNumber)
        {
            var loan = await _loanRepository.GetByAccountNumber(accountNumber);
            if (loan == null)
            {
                return null!;
            }

            return loan;
        }

        public async Task<List<LoanViewModel>> GetAllByClientId(int clientId)
        {
            var loans = await _loanRepository.GetAllAsync();

            var loanViewModels = new List<LoanViewModel>();

            foreach (var loan in loans.Where(b => b.ClientId == clientId))
            {


                var loanViewModel = new LoanViewModel
                {
                    ClientId = loan.ClientId,
                    Id = loan.Id,
                    Balance = loan.Balance,
                    DateCreated = loan.DateCreated,
                    Amount = loan.Amount

                };

                loanViewModels.Add(loanViewModel);
            }

            return loanViewModels;
        }
        public async Task<Loan> GetByAccountNumberLoggedUser(string accountNumber, int ClientId)
        {
            var loan = await _loanRepository.GetByAccountNumberLoggedUser(accountNumber, ClientId);
            if (loan == null)
            {
                return null!;
            }

            return loan;
        }
        #endregion

        #region Update 
        public async Task UpdateLoan(double balance, double amount, string accountNumber, int clientId)
        {
            var loan = await GetByAccountNumberLoggedUser(accountNumber, clientId);

            SaveLoanViewModel vm = new SaveLoanViewModel
            {
                Id = loan.Id,
                ClientId = loan.ClientId,
                DateCreated = loan.DateCreated,
                Balance = balance,
                Amount = amount,
            };

            await base.UpdateProduct(vm, accountNumber);
        }
        #endregion
    }
}
