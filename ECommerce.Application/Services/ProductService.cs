using AutoMapper;
using Ecommerce.Middleware.Common;
using ECommerce.Application.DTOs;
using ECommerce.Domain;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.UnitOfWork;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);
            if (product == null)
                throw ApiException.ProductNotFound(id);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync(ProductQuery filter)
        {
            var products = await _unitOfWork.Products.GetProductsAsync(filter);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
                throw ApiException.CategoryNotFound(createProductDto.CategoryId);

            var product = _mapper.Map<Product>(createProductDto);
            product.CreatedDate = DateTime.UtcNow;

            if (createProductDto.ImageUrl != null)
            {
                product.ImageUrl = await _imageService.UploadImageAsync(createProductDto.ImageUrl);
            }

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            var createdProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.Id);
            return _mapper.Map<ProductDTO>(createdProduct);
        }

        public async Task UpdateProductAsync(int id, CreateProductDTO updateProductDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw ApiException.ProductNotFound(id);

            if (updateProductDto.CategoryId != 0)
            {
                var categoryExists = await _unitOfWork.Categories.ExistsAsync(updateProductDto.CategoryId);
                if (!categoryExists)
                    throw ApiException.CategoryNotFound(updateProductDto.CategoryId);
            }

            if (updateProductDto.RemoveImage && !string.IsNullOrEmpty(product.ImageUrl))
            {
                await _imageService.DeleteImageAsync(product.ImageUrl);
                product.ImageUrl = null;
            }

            if (updateProductDto.ImageUrl != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                    await _imageService.DeleteImageAsync(product.ImageUrl);

                product.ImageUrl = await _imageService.UploadImageAsync(updateProductDto.ImageUrl);
            }

            if (updateProductDto.Name != null)
                product.Name = updateProductDto.Name;

            if (updateProductDto.Description != null)
                product.Description = updateProductDto.Description;

            if (updateProductDto.Price.HasValue)
                product.Price = updateProductDto.Price.Value;

            if (updateProductDto.Stock.HasValue)
                product.Stock = updateProductDto.Stock.Value;

            if (updateProductDto.CategoryId != 0)
                product.CategoryId = updateProductDto.CategoryId;

            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw ApiException.ProductNotFound(id);
            if (!string.IsNullOrEmpty(product.ImageUrl))
                await _imageService.DeleteImageAsync(product.ImageUrl);

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
        }
    }
}
