using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task DeleteImageAsync(string imageUrl);
    }
}
