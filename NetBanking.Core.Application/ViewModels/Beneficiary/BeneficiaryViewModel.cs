using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public int ClientId { get; set; }

        public string BeneficiaryName { get; set; }
        public string BeneficiaryLastName { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
    }
}
