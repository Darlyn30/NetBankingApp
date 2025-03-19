using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetByAccountNumber(string accountNumber);
    }
}
