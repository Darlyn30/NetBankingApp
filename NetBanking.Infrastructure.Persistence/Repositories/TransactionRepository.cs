using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationContext _context;
        public TransactionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
