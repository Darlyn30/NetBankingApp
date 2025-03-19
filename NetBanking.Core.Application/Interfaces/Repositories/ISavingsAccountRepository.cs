using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface ISavingsAccountRepository : IGenericRepository<SavingsAccount>
    {
        Task<SavingsAccount> GetByAccountNumberLoggedUser(string accountNumber, int clientId);
        Task<SavingsAccount> GetByAccountNumber(string accountNumber);
    }
}
