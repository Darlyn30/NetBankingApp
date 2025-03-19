using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string OriginAccount { get; set; }
        public string DestinationAccount { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public double Amount { get; set; }
        public string? Concept { get; set; }
    }
}
