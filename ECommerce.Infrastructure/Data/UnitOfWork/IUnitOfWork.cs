using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        Task<int> SaveAsync();
    }
}
