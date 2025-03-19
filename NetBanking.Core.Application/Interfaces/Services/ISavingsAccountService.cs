using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface ISavingsAccountService : IGenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>
    {
        Task<SavingsAccount> GetByAccountNumberLoggedUser(string accountNumber, int ClientId);
        Task<SavingsAccount> GetByAccountNumber(string accountNumber);
        Task<List<SavingsAccountViewModel>> GetAllByClientId(int clientId);
        Task UpdateSavingsAccount(double balance, int ClientId, string id);
        Task<SavingsAccountViewModel> GetClientMainAccount(int ClientId);
        Task Delete(string id);
    }
}
