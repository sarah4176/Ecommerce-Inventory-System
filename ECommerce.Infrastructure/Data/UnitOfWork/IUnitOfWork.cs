using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        Task<int> SaveAsync();
    }
}
