using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public virtual async Task DeleteAsync(T model)
        {
            _context.Set<T>().Remove(model);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(T model, int id)
        {
            var entry = await _context.Set<T>().FindAsync(id);
            _context.Entry(entry).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateProductAsync(T model, string id)
        {
            var entry = await _context.Set<T>().FindAsync(id);
            _context.Entry(entry).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
        }
    }
}
