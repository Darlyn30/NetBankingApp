using NetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class CreditCard : ProductBaseEntity
    {
        public double Limit { get; set; }
        public double Debit { get; set; }
    }
}
