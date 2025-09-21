using ECommerce.Domain;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductQuery filter)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
                query = query.Where(p => p.Name.Contains(filter.SearchTerm) ||
                                        p.Description.Contains(filter.SearchTerm));

            query = query.Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);

            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
