using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            return await _context.Products
                .CountAsync(p => p.CategoryId == categoryId);
        }
        public async Task<IEnumerable<Category>> SearchAsync(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(searchTerm) ||
                           (c.Description != null && c.Description.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
