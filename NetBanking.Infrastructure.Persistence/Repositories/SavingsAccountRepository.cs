using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class SavingsAccountRepository : GenericRepository<SavingsAccount>, ISavingsAccountRepository
    {
        private readonly ApplicationContext _context;

        public SavingsAccountRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public Task<SavingsAccount> GetByAccountNumber(string accountNumber)
        {
            var savingsAccount = _context.SavingsAccounts.FirstOrDefaultAsync(b => b.Id == accountNumber);

            if (savingsAccount == null)
            {
                return null;
            }

            return savingsAccount;
        }

        public Task<SavingsAccount> GetByAccountNumberLoggedUser(string accountNumber, int clientId)
        {
            var savingsAccount = _context.SavingsAccounts.FirstOrDefaultAsync(b => b.Id == accountNumber && b.ClientId == clientId);

            if (savingsAccount == null)
            {
                return null;
            }

            return savingsAccount;
        }
    }
}
