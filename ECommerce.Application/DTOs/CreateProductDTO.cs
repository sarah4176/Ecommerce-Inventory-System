using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTOs
{
    public class CreateProductDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public bool RemoveImage { get; set; }
    }
}
