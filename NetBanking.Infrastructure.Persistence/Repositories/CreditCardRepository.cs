using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
    {
        private readonly ApplicationContext _context;

        public CreditCardRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public Task<CreditCard> GetByAccountNumber(string accountNumber)
        {
            var creditCard = _context.CreditCards.FirstOrDefaultAsync(b => b.Id == accountNumber);
            if (creditCard == null)
            {
                return null;
            }

            return creditCard;
        }

        public Task<CreditCard> GetByAccountNumberLoggedUser(string accountNumber, int clientId)
        {
            var creditCard = _context.CreditCards.FirstOrDefaultAsync(b => b.Id == accountNumber && b.ClientId == clientId);
            if (creditCard == null)
            {
                return null;
            }

            return creditCard;
        }
    }
}
