using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Enums
{
    public enum TransactionType
    {
        AccountTransfer = 1,
        LoanPayment = 2,
        ExpressPayment = 3,
        CreditCardPayment = 4,
        CashAdvance = 5,
        BeneficiaryPayment = 6
    }
}
