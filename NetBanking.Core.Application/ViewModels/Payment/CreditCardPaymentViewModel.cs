using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.SavingsAccount;

namespace NetBanking.Core.Application.ViewModels.Payment
{
    public class CreditCardPaymentViewModel
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public double Amount { get; set; }
        public List<SavingsAccountViewModel>? LoggedUserAccounts { get; set; }
        public List<CreditCardViewModel>? LoggedUserCreditCards { get; set; }
    }
}
