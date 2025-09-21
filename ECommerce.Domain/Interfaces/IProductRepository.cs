using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsAsync(ProductQuery filter);
        Task<Product> GetByIdWithCategoryAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
