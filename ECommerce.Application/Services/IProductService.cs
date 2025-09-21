using ECommerce.Application.DTOs;
using ECommerce.Domain;

namespace ECommerce.Application.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductQuery filter);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDto);
        Task UpdateProductAsync(int id, CreateProductDTO updateProductDto);
        Task DeleteProductAsync(int id);
    }
}
