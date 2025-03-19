using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.SavingsAccount
{
    public class SavingsAccountViewModel
    {
        public string Id { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public DateTime DateCreated { get; set; }
        public double Balance { get; set; }
        public bool IsMainAccount { get; set; }
    }
}
