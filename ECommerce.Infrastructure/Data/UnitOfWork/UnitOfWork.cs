using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data.Repositories;

namespace ECommerce.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(ApplicationDbContext context, ICategoryRepository categoryRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
        public IUserRepository Users=> _userRepository ??= new UserRepository(_context);

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
