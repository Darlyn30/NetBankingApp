using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.SavingsAccount;

namespace NetBanking.Core.Application.ViewModels.Payment
{
    public class BeneficiaryPaymentViewModel
    {
        public string Destination { get; set; }
        public string Origin { get; set; }

        public double Amount { get; set; }
        public List<SavingsAccountViewModel>? LoggedUserAccounts { get; set; }

        public List<BeneficiaryViewModel>? LoggedUserBeneficiaries { get; set; }

        public string? BeneficiaryFirstName { get; set; }
        public string? BeneficiaryLastName { get; set; }
    }
}
