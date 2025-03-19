using NetBanking.Core.Application.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface ITransfersService
    {
        Task<SaveTransactionViewModel> Transfer(SaveTransactionViewModel vm, Enums.TransactionType transactionType, bool isCashAdvance);
        Task<bool> AddMoneyToAccount(string accountNumberDestination, string accountNumberOrigin, double amount);
        Task<bool> AddMoneyToAccountCreditCard(string accountNumberOrigin, string accountNumberDestination, double amount);
        Task<bool> CashAdvance(SaveTransactionViewModel vm);
    }
}
