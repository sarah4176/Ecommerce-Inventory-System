using ECommerce.Application.DTOs;

namespace ECommerce.Application.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto);
        Task UpdateCategoryAsync(int id, CreateCategoryDTO updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
