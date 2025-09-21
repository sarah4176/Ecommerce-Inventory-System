using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<int> GetProductCountAsync(int categoryId);
        Task<IEnumerable<Category>> SearchAsync(string searchTerm);
    }
}
