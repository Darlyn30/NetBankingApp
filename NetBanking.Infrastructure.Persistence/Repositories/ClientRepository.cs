using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly ApplicationContext _context;
        public ClientRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async  Task<Client> GetByAccountNumber(string accountNumber)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.SavingsAccounts.Any(s => s.Id == accountNumber));

            return client;
        }
    }
}
