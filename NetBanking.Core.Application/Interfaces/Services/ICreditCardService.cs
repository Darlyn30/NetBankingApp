using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface ICreditCardService : IGenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>
    {
        Task<List<CreditCardViewModel>> GetAllByClientId(int clientId);

        Task<CreditCard> GetByAccountNumberLoggedUser(string accountNumber, int clientId);
        Task<CreditCard> GetByAccountNumber(string accountNumber);

        Task UpdateCreditCard(double balance, double debit, string accountNumber, int clientId);
        Task DeleteProduct(string id);
    }
}
