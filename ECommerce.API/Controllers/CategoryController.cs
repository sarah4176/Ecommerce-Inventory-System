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
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDTO>>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(ApiResponse<IEnumerable<CategoryDTO>>.SuccessResponse(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(ApiResponse<CategoryDTO>.SuccessResponse(category));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> Create([FromBody] CreateCategoryDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<CategoryDTO>.ErrorResponse("Invalid input data", 400));

            var category = await _categoryService.CreateCategoryAsync(createDto);

            return CreatedAtAction(nameof(GetById),
                new { id = category.Id },
                ApiResponse<CategoryDTO>.SuccessResponse(category, "Category created successfully", 201));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _categoryService.UpdateCategoryAsync(id, updateDto);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent(); 
        }
    }
}
