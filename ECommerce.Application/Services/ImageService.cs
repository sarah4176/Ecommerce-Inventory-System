using Ecommerce.Middleware.Common;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services
{
        public class ImageService : IImageService
        {
            private readonly string _webRootPath;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public ImageService(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            public async Task<string> UploadImageAsync(IFormFile imageFile)
            {
                if (imageFile == null || imageFile.Length == 0)
                    throw ApiException.BadRequest("No image file provided");

                if (imageFile.Length > 5 * 1024 * 1024)
                    throw ApiException.BadRequest("Image file is too large (max 5MB)");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                    throw ApiException.BadRequest("Invalid image file type. Allowed types: JPG, JPEG, PNG, GIF, WEBP");

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_webRootPath, "uploads", "products");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host}/uploads/products/{fileName}";
            }

            public Task DeleteImageAsync(string imageUrl)
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return Task.CompletedTask;

                var fileName = Path.GetFileName(new Uri(imageUrl).AbsolutePath);

                if (string.IsNullOrEmpty(fileName))
                    throw ApiException.BadRequest("Invalid image URL format");

                var filePath = Path.Combine(_webRootPath, "uploads", "products", fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return Task.CompletedTask;
            }
        }
}
