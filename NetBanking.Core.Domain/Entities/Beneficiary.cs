using NetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class Beneficiary : AuditableBaseEntity
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public string SavingsAccountId { get; set; } // product 
        public SavingsAccount? SavingsAccount { get; set; }
    }
}
