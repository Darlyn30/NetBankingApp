
namespace NetBanking.Core.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string Origin { get; set; } // product where the money comes from
        public string Destination { get; set; } // product where the money goes to

        public TransactionType? TransactionType { get; set; }
        public int TransactionTypeId { get; set; }
        public double Amount { get; set; }
        public string? Concept { get; set; } // a brief description of the transaction
    }
}
