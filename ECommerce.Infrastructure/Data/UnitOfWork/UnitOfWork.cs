using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data.Repositories;

namespace ECommerce.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICategoryRepository _categoryRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
