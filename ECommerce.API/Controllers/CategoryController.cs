using Ecommerce.Middleware.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse>> GetAll() 
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories); 
        }

        [HttpGet("GetCategoryById")]
        public async Task<ActionResult<ApiResponse>> GetById(int id) 
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse.ErrorResponse("Category not found", 404));

            return Ok(category); 
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateCategoryDTO createDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            var category = await _categoryService.CreateCategoryAsync(createDto);

            return CreatedAtAction(nameof(GetById),
                new { id = category.Id },
                ApiResponse.SuccessResponse(category, "Category created successfully", 201)); 
        }


        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            await _categoryService.UpdateCategoryAsync(id, updateDto);
            return NoContent();
        }
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
        [HttpGet("SearchCategory")]
        public async Task<ActionResult<ApiResponse>> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(ApiResponse.ErrorResponse("Search query is required", 400));

            var categories = await _categoryService.SearchCategoriesAsync(keyword);
            return Ok(ApiResponse.SuccessResponse(categories));
        }
    }
}
