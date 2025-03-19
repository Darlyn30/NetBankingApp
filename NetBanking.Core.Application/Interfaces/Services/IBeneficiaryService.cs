using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        Task<SaveBeneficiaryViewModel> AddBeneficiary(SaveBeneficiaryViewModel vm, string AccountNumber);

        Task<List<BeneficiaryViewModel>> GetAllByClientId(int clientId);

        Task<SavingsAccount> GetByAccountNumber(string accountNumber);

        Task<Beneficiary> GetBeneficiary(string accountNumber);

        Task DeleteBeneficiary(string AccountNumber);
    }
}
