using NetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class SavingsAccount : ProductBaseEntity
    {
        public bool IsMainAccount { get; set; }
        public ICollection<Beneficiary>? Beneficiaries { get; set; }
    }
}
