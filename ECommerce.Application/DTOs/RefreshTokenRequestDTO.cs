using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
