using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int ClientId { get; set; }
        public string Username { get; internal set; }
        public List<SavingsAccountViewModel> SavingsAccounts { get; set; }
        public List<LoanViewModel> Loans { get; set; }
        public List<CreditCardViewModel> CreditCards { get; set; }
    }
}
