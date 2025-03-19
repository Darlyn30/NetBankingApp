using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T model);
        Task UpdateAsync(T model, int id);
        Task UpdateProductAsync(T model, string id);
        Task DeleteAsync(T model);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllWithIncludeAsync(List<string> properties);
    }
}
