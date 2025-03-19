
namespace NetBanking.Core.Domain.Entities
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
