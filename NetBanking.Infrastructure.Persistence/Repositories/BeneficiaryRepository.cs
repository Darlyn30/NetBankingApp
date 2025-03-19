using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _context;

        public BeneficiaryRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public Task<Beneficiary> GetBeneficiary(string accountNumber)
        {
            var beneficiary = _context.Beneficiaries.FirstOrDefaultAsync(b => b.SavingsAccountId == accountNumber);
            if (beneficiary == null)
            {
                return null;
            }

            return beneficiary;
        }

        public Task<SavingsAccount> GetByAccountNumber(string accountNumber)
        {
            var sa = _context.SavingsAccounts.FirstOrDefaultAsync(b => b.Id == accountNumber);
            if (sa == null)
            {
                return null;
            }

            return sa;
        }
    }
}
