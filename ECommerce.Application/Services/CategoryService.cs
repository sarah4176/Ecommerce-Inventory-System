using AutoMapper;
using Ecommerce.Middleware.Common;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.UnitOfWork;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw ApiException.CategoryNotFound(id);

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            categoryDto.ProductCount = await _unitOfWork.Categories.GetProductCountAsync(id);

            return categoryDto;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var categoryDtos = await Task.WhenAll(
                categories.Select(async category =>
                {
                    var categoryDto = _mapper.Map<CategoryDTO>(category);
                    categoryDto.ProductCount = await _unitOfWork.Categories.GetProductCountAsync(category.Id);
                    return categoryDto;
                })
            );

            return categoryDtos;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto)
        {
            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(createCategoryDto.Name);
            if (existingCategory != null)
                throw ApiException.CategoryAlreadyExists(createCategoryDto.Name);

            var category = _mapper.Map<Category>(createCategoryDto);
            category.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            var createdCategoryDto = _mapper.Map<CategoryDTO>(category);
            createdCategoryDto.ProductCount = 0; 

            return createdCategoryDto;
        }

        public async Task UpdateCategoryAsync(int id, CreateCategoryDTO updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw ApiException.CategoryNotFound(id);

            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(updateCategoryDto.Name);
            if (existingCategory != null && existingCategory.Id != id)
                throw ApiException.CategoryAlreadyExists(updateCategoryDto.Name);

            _mapper.Map(updateCategoryDto, category);
            category.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw ApiException.CategoryNotFound(id);

            var productCount = await _unitOfWork.Categories.GetProductCountAsync(id);
            if (productCount > 0)
                throw ApiException.Conflict($"Cannot delete category with {productCount} associated products");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IEnumerable<CategoryDTO>> SearchCategoriesAsync(string searchTerm)
        {
            var categories = await _unitOfWork.Categories.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
    }
}
