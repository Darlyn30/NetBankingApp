using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        Task<Loan> GetByAccountNumberLoggedUser(string accountNumber, int clientId);
        Task<Loan> GetByAccountNumber(string accountNumber);
    }
}
