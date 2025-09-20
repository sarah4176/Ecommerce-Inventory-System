using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }
    }

}
