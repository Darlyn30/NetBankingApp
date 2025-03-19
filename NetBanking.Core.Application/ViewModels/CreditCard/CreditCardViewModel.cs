using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class CreditCardViewModel
    {
        public string Id { get; set; }
        public int ClientId { get; set; }
        public DateTime DateCreated { get; set; }
        public double Balance { get; set; }
        public double Limit { get; set; }
        public double Debit { get; set; }
    }
}
