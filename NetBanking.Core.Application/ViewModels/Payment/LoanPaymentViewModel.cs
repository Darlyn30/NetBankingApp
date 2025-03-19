using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Payment
{
    public class LoanPaymentViewModel
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public double Amount { get; set; }
        public List<LoanViewModel>? LoggedUserLoans { get; set; }

        public List<SavingsAccountViewModel>? LoggedUserAccounts { get; set; }
    }
}
