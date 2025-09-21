using Ecommerce.Middleware.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Services;
using ECommerce.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] ProductQuery filter)
        {
            var products = await _productService.GetProductsAsync(filter);
            return Ok(ApiResponse.SuccessResponse(products));
        }

        [HttpGet("GetProductById")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(ApiResponse.ErrorResponse("Product not found", 404));

            return Ok(ApiResponse.SuccessResponse(product));
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] CreateProductDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            var product = await _productService.CreateProductAsync(createDto);

            return CreatedAtAction(nameof(GetById),
                new { id = product.Id },
                ApiResponse.SuccessResponse(product, "Product created successfully", 201));
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateProductDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            await _productService.UpdateProductAsync(id, updateDto);
            return NoContent();
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
