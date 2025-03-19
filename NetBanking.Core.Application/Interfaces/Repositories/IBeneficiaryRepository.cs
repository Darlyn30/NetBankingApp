using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface IBeneficiaryRepository : IGenericRepository<Beneficiary>
    {
        Task<SavingsAccount> GetByAccountNumber(string accountNumber);
        Task<Beneficiary> GetBeneficiary(string accountNumber);
    }
}
