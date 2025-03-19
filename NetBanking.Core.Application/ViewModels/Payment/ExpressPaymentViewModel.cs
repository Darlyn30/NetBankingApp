using NetBanking.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Payment
{
    public class ExpressPaymentViewModel
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public double Amount { get; set; }
        public List<SavingsAccountViewModel>? LoggedUserAccounts { get; set; }

        public string? ClientName { get; set; }
        public string? ClientLastName { get; set; }
    }
}
