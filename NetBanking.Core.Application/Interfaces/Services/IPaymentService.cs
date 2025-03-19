using NetBanking.Core.Application.ViewModels.Payment;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ExpressPaymentViewModel> ExpressPayment(ExpressPaymentViewModel vm);
        Task<CreditCardPaymentViewModel> CreditCardPayment(CreditCardPaymentViewModel vm);
        Task<LoanPaymentViewModel> LoanPayment(LoanPaymentViewModel vm);
        Task<BeneficiaryPaymentViewModel> BeneficiaryPayment(BeneficiaryPaymentViewModel vm);
    }
}
